using RaizesNordeste.Application.Common;
using RaizesNordeste.Application.DTOs.Pedidos;
using RaizesNordeste.Application.Mappers;
using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Services;
using RaizesNordeste.Domain.Validators;

namespace RaizesNordeste.Application.UseCases.Pedidos;

public class CriarPedidoUseCase(
    IPedidoRepository pedidoRepository,
    ICardapioUnidadeRepository cardapioRepository,
    IEstoqueRepository estoqueRepository,
    IMovimentacaoEstoqueRepository movimentacaoEstoqueRepository,
    IAuditoriaRepository auditoriaRepository,
    IUnidadeRepository unidadeRepository,
    IUsuarioRepository usuarioRepository,
    IClienteRepository clienteRepository,
    IPromocaoRepository promocaoRepository,
    IUnitOfWork unitOfWork,
    EstoqueDomainService estoqueDomainService,
    PromocaoDomainService promocaoDomainService,
    PedidoDomainService pedidoDomainService)
{
    public async Task<ResultadoOperacao<PedidoResponse>> ExecutarAsync(
        CriarPedidoRequest request,
        UsuarioAutenticado? usuarioAutenticado = null,
        CancellationToken cancellationToken = default)
    {
        var titularidade = await AplicarTitularidadeClienteAsync(request, usuarioAutenticado, cancellationToken);
        if (titularidade is not null)
        {
            return titularidade;
        }

        await ValidarPrecondicoesAsync(request, cancellationToken);

        var usuarioOperacao = await ObterUsuarioOperacaoAsync(cancellationToken);
        var pedido = pedidoDomainService.CriarPedido(request.ClienteId, request.UnidadeId, request.CanalPedido);
        var cardapioItens = await cardapioRepository.ListarPorUnidadeAsync(request.UnidadeId, cancellationToken);
        var estoquesParaBaixar = new List<(Estoque Estoque, int Quantidade)>();

        foreach (var itemRequest in request.Itens)
        {
            var cardapioItem = cardapioItens.FirstOrDefault(c => c.ProdutoId == itemRequest.ProdutoId);
            pedidoDomainService.ValidarDisponibilidadeNoCardapio(cardapioItem, itemRequest.ProdutoId);

            var estoque = await estoqueRepository.ObterPorUnidadeEProdutoAsync(request.UnidadeId, itemRequest.ProdutoId, cancellationToken);
            pedidoDomainService.ValidarEstoqueExistente(estoque, itemRequest.ProdutoId);

            if (estoque!.QuantidadeAtual < itemRequest.Quantidade)
            {
                throw new DomainException($"Estoque insuficiente. Disponível: {estoque.QuantidadeAtual}, Requerido: {itemRequest.Quantidade}.");
            }

            estoquesParaBaixar.Add((estoque, itemRequest.Quantidade));

            pedido.AdicionarItem(itemRequest.ProdutoId, itemRequest.Quantidade, cardapioItem!.Preco);
        }

        var (valorBase, descontoTotal, promocaoAplicada) = await AplicarPromocaoAsync(pedido, cancellationToken);

        pedido.Validar();

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            foreach (var (estoque, quantidade) in estoquesParaBaixar)
            {
                estoqueDomainService.BaixarEstoque(estoque, quantidade);
                await estoqueRepository.UpdateAsync(estoque, cancellationToken);

                var movimentacao = pedidoDomainService.CriarMovimentacaoVenda(estoque.Id, usuarioOperacao.Id, quantidade, 0);
                new MovimentacaoEstoqueValidator().ValidaOuLancaExcecao(movimentacao);
                await movimentacaoEstoqueRepository.InsertAsync(movimentacao, cancellationToken);
            }

            var pedidoSalvo = await pedidoRepository.InsertAsync(pedido, cancellationToken);

            await RegistrarAuditoriaAsync(pedidoSalvo, request, valorBase, descontoTotal, promocaoAplicada, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return ResultadoOperacao<PedidoResponse>.Criado(UtilPedidoMapper.MapearParaResponse(pedidoSalvo));
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<ResultadoOperacao<PedidoResponse>?> AplicarTitularidadeClienteAsync(
        CriarPedidoRequest request,
        UsuarioAutenticado? usuarioAutenticado,
        CancellationToken cancellationToken)
    {
        if (usuarioAutenticado?.EhCliente != true)
        {
            return null;
        }

        if (!usuarioAutenticado.UsuarioId.HasValue)
        {
            return ResultadoOperacao<PedidoResponse>.Proibido(
                "Cliente correspondente não encontrado no sistema.",
                "CLIENTE_NAO_ENCONTRADO");
        }

        var clienteLogado = await clienteRepository.ObterPorUsuarioIdAsync(usuarioAutenticado.UsuarioId.Value, cancellationToken);
        if (clienteLogado is null)
        {
            return ResultadoOperacao<PedidoResponse>.Proibido(
                "Cliente correspondente não encontrado no sistema.",
                "CLIENTE_NAO_ENCONTRADO");
        }

        if (!request.ClienteId.HasValue)
        {
            request.ClienteId = clienteLogado.Id;
            return null;
        }

        return request.ClienteId.Value == clienteLogado.Id
            ? null
            : ResultadoOperacao<PedidoResponse>.Proibido(
                "Operação não permitida: o cliente autenticado só pode criar pedidos para si mesmo.",
                "TITULARIDADE_INVALIDA");
    }

    private async Task ValidarPrecondicoesAsync(CriarPedidoRequest request, CancellationToken cancellationToken)
    {
        var unidade = await unidadeRepository.GetByIdAsync(request.UnidadeId, cancellationToken);
        pedidoDomainService.ValidarUnidadeAtiva(unidade);

        if (request.ClienteId.HasValue)
        {
            var cliente = await clienteRepository.GetByIdAsync(request.ClienteId.Value, cancellationToken);
            pedidoDomainService.ValidarClienteAtivo(cliente);
        }

        pedidoDomainService.ValidarItensPresentes(request.Itens);
    }

    private async Task<Usuario> ObterUsuarioOperacaoAsync(CancellationToken cancellationToken)
    {
        var ativos = await usuarioRepository.ListAsync(u => u.Ativo, take: 1, cancellationToken: cancellationToken);
        var usuario = ativos.FirstOrDefault();
        pedidoDomainService.ValidarUsuarioAtivo(usuario);
        return usuario!;
    }

    private async Task<(decimal ValorBase, decimal DescontoTotal, Promocao? PromocaoAplicada)>
        AplicarPromocaoAsync(Pedido pedido, CancellationToken cancellationToken)
    {
        var valorBase = pedido.Itens.Sum(i => i.ValorTotal);
        var promocoesAtivas = await promocaoRepository.ListarAtivasVigentesAsync(DateTime.UtcNow, cancellationToken);
        var (descontoTotal, promocaoAplicada) = promocaoDomainService.CalcularDesconto(pedido, promocoesAtivas);

        pedido.AplicarDesconto(descontoTotal);

        return (valorBase, descontoTotal, promocaoAplicada);
    }

    private async Task RegistrarAuditoriaAsync(
        Pedido pedidoSalvo,
        CriarPedidoRequest request,
        decimal valorBase,
        decimal descontoTotal,
        Promocao? promocaoAplicada,
        CancellationToken cancellationToken)
    {
        var auditoriaCriacao = new Auditoria
        {
            UsuarioId = null,
            Entidade = "Pedido",
            EntidadeId = pedidoSalvo.Id,
            Acao = "CRIAR",
            DadosNovos = $"Pedido criado no canal {request.CanalPedido}. Valor base: {valorBase}. Valor final: {pedidoSalvo.ValorTotal}",
            CriadoEm = DateTime.UtcNow
        };
        new AuditoriaValidator().ValidaOuLancaExcecao(auditoriaCriacao);
        await auditoriaRepository.InsertAsync(auditoriaCriacao, cancellationToken);

        if (descontoTotal > 0 && promocaoAplicada is not null)
        {
            var auditoriaDesconto = new Auditoria
            {
                UsuarioId = null,
                Entidade = "Pedido",
                EntidadeId = pedidoSalvo.Id,
                Acao = "APLICAR_DESCONTO",
                DadosNovos = $"Desconto de {descontoTotal} aplicado via promoção '{promocaoAplicada.Nome}' (ID: {promocaoAplicada.Id}).",
                CriadoEm = DateTime.UtcNow
            };
            new AuditoriaValidator().ValidaOuLancaExcecao(auditoriaDesconto);
            await auditoriaRepository.InsertAsync(auditoriaDesconto, cancellationToken);
        }
    }
}
