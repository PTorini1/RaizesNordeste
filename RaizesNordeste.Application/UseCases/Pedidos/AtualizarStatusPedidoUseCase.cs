using RaizesNordeste.Application.DTOs.Pedidos;
using RaizesNordeste.Application.Mappers;
using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Services;
using RaizesNordeste.Domain.Validators;

namespace RaizesNordeste.Application.UseCases.Pedidos;

public class AtualizarStatusPedidoUseCase(
    IPedidoRepository pedidoRepository,
    IEstoqueRepository estoqueRepository,
    IMovimentacaoEstoqueRepository movimentacaoEstoqueRepository,
    IAuditoriaRepository auditoriaRepository,
    IUsuarioRepository usuarioRepository,
    IUnitOfWork unitOfWork,
    PedidoDomainService pedidoDomainService,
    EstoqueDomainService estoqueDomainService)
{
    public async Task<PedidoResponse> ExecutarAsync(int pedidoId, StatusPedido novoStatus, CancellationToken cancellationToken = default)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var (pedido, statusAnterior) = await ObterEAlterarStatusAsync(pedidoId, novoStatus, cancellationToken);

            if (pedido is null)
            {
                var pedidoSemMudanca = await pedidoRepository.ObterPorIdComItensAsync(pedidoId, cancellationToken);
                await unitOfWork.CommitAsync(cancellationToken);
                return UtilPedidoMapper.MapearParaResponse(pedidoSemMudanca!);
            }

            if (novoStatus == StatusPedido.Cancelado && statusAnterior != StatusPedido.Cancelado)
            {
                await EstornarEstoqueAsync(pedido, cancellationToken);
            }

            await pedidoRepository.UpdateAsync(pedido, cancellationToken);
            await RegistrarAuditoriaAsync(pedido, statusAnterior, novoStatus, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);
            return UtilPedidoMapper.MapearParaResponse(pedido);
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<(Pedido? Pedido, StatusPedido StatusAnterior)> ObterEAlterarStatusAsync(
        int pedidoId,
        StatusPedido novoStatus,
        CancellationToken cancellationToken)
    {
        var pedido = await pedidoRepository.ObterPorIdComItensAsync(pedidoId, cancellationToken);
        _ = pedido ?? throw new DomainException($"Pedido ID {pedidoId} não encontrado.");

        var statusAnterior = pedido.Status;

        if (statusAnterior == novoStatus)
        {
            return (null, statusAnterior);
        }

        pedido.AlterarStatus(novoStatus);
        return (pedido, statusAnterior);
    }

    private async Task EstornarEstoqueAsync(Pedido pedido, CancellationToken cancellationToken)
    {
        var usuariosAtivos = await usuarioRepository.ListAsync(u => u.Ativo, take: 1, cancellationToken: cancellationToken);
        var usuarioOperacao = usuariosAtivos.FirstOrDefault();
        pedidoDomainService.ValidarUsuarioAtivo(usuarioOperacao);

        foreach (var item in pedido.Itens)
        {
            var estoque = await estoqueRepository.ObterPorUnidadeEProdutoAsync(
                pedido.UnidadeId, item.ProdutoId, cancellationToken);

            if (estoque is null) continue;

            estoqueDomainService.ReporEstoque(estoque, item.Quantidade);
            await estoqueRepository.UpdateAsync(estoque, cancellationToken);

            var movEstoque = new MovimentacaoEstoque
            {
                EstoqueId = estoque.Id,
                UsuarioId = usuarioOperacao!.Id,
                TipoMovimentacao = TipoMovimentacaoEstoque.Ajuste,
                Quantidade = item.Quantidade,
                Motivo = $"Cancelamento do Pedido ID {pedido.Id}",
                CriadoEm = DateTime.UtcNow
            };
            new MovimentacaoEstoqueValidator().ValidaOuLancaExcecao(movEstoque);
            await movimentacaoEstoqueRepository.InsertAsync(movEstoque, cancellationToken);
        }
    }

    private async Task RegistrarAuditoriaAsync(Pedido pedido, StatusPedido statusAnterior, StatusPedido novoStatus, CancellationToken cancellationToken)
    {
        var auditoria = new Auditoria
        {
            UsuarioId = null,
            Entidade = "Pedido",
            EntidadeId = pedido.Id,
            Acao = "ALTERAR_STATUS",
            DadosAnteriores = $"Status anterior: {statusAnterior}",
            DadosNovos = $"Status novo: {novoStatus}",
            CriadoEm = DateTime.UtcNow
        };
        new AuditoriaValidator().ValidaOuLancaExcecao(auditoria);
        await auditoriaRepository.InsertAsync(auditoria, cancellationToken);
    }
}
