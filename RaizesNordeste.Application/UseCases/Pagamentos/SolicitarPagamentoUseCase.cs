using RaizesNordeste.Application.Common;
using RaizesNordeste.Application.DTOs.Pagamentos;
using RaizesNordeste.Application.UseCases.Fidelidade;
using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Interfaces.Services;
using RaizesNordeste.Domain.Services;
using RaizesNordeste.Domain.Validators;

namespace RaizesNordeste.Application.UseCases.Pagamentos;

public class SolicitarPagamentoUseCase(
    IPedidoRepository pedidoRepository,
    IPagamentoRepository pagamentoRepository,
    IPagamentoMockService pagamentoMockService,
    IRedisCacheService redisCacheService,
    IAuditoriaRepository auditoriaRepository,
    IClienteRepository clienteRepository,
    IUnitOfWork unitOfWork,
    AplicarPontosFidelidadeUseCase aplicarPontosFidelidadeUseCase,
    PagamentoDomainService pagamentoDomainService)
{
    public async Task<ResultadoOperacao<PagamentoResponse>> ExecutarAsync(
        SolicitarPagamentoRequest request,
        UsuarioAutenticado? usuarioAutenticado = null,
        CancellationToken cancellationToken = default)
    {
        var pedido = await ObterEValidarPedidoAsync(request.PedidoId, cancellationToken);

        if (usuarioAutenticado?.EhCliente == true)
        {
            var permitido = await ClientePodeAcessarPedidoAsync(usuarioAutenticado, pedido, cancellationToken);
            if (!permitido)
            {
                return ResultadoOperacao<PagamentoResponse>.Proibido(
                    "Operação não permitida: o cliente autenticado só pode pagar os seus próprios pedidos.",
                    "TITULARIDADE_INVALIDA");
            }
        }

        var pagamentoIdempotente = await ResolverIdempotenciaAsync(request, cancellationToken);
        if (pagamentoIdempotente is not null)
        {
            return ResultadoOperacao<PagamentoResponse>.Sucesso(MapearParaResponse(pagamentoIdempotente));
        }

        var pagamento = await IniciarPagamentoAsync(request, pedido, cancellationToken);
        var result = await ProcessarComProvedorAsync(pagamento, pedido, request.Provedor, cancellationToken);

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await AtualizarStatusPosProcessamentoAsync(pagamento, pedido, result, request.Provedor, cancellationToken);
            await RegistrarAuditoriaAsync(pagamento, pedido, result, request.Provedor, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }

        return ResultadoOperacao<PagamentoResponse>.Criado(MapearParaResponse(pagamento));
    }

    private async Task<bool> ClientePodeAcessarPedidoAsync(
        UsuarioAutenticado usuarioAutenticado,
        Pedido pedido,
        CancellationToken cancellationToken)
    {
        if (!usuarioAutenticado.UsuarioId.HasValue || !pedido.ClienteId.HasValue)
        {
            return false;
        }

        var cliente = await clienteRepository.ObterPorUsuarioIdAsync(usuarioAutenticado.UsuarioId.Value, cancellationToken);
        return cliente is not null && cliente.Id == pedido.ClienteId.Value;
    }

    private async Task<Pedido> ObterEValidarPedidoAsync(int pedidoId, CancellationToken cancellationToken)
    {
        var pedido = await pedidoRepository.ObterPorIdComItensAsync(pedidoId, cancellationToken);
        pagamentoDomainService.ValidarPedidoParaPagamento(pedido, pedidoId);
        return pedido!;
    }

    private async Task<Pagamento?> ResolverIdempotenciaAsync(
        SolicitarPagamentoRequest request,
        CancellationToken cancellationToken)
    {
        if ((await pedidoRepository.ObterPorIdComItensAsync(request.PedidoId, cancellationToken))?.Status
            == Domain.Enums.StatusPedido.PagamentoAprovado)
        {
            var aprovado = await pagamentoRepository.ObterPorPedidoIdAsync(request.PedidoId, cancellationToken);
            if (aprovado is not null)
            {
                return aprovado;
            }
        }

        var lockKey = $"pagamento:idempotencia:pedido:{request.PedidoId}";
        var lockAdquirido = await redisCacheService.SetStringWithLockAsync(
            lockKey, request.ChaveIdempotencia, TimeSpan.FromMinutes(10), cancellationToken);

        if (!lockAdquirido)
        {
            var existente = await pagamentoRepository.ObterPorPedidoIdAsync(request.PedidoId, cancellationToken);
            if (existente is not null && existente.ChaveIdempotencia == request.ChaveIdempotencia)
            {
                return existente;
            }

            pagamentoDomainService.ValidarIdempotencia(existente, request.ChaveIdempotencia);
        }

        return null;
    }

    private async Task<Pagamento> IniciarPagamentoAsync(SolicitarPagamentoRequest request, Pedido pedido, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var novoPagamento = pagamentoDomainService.CriarPagamento(request.PedidoId, request.Provedor, request.ChaveIdempotencia);

            var pagamento = await pagamentoRepository.InsertAsync(novoPagamento, cancellationToken);

            pedido.AlterarStatus(Domain.Enums.StatusPedido.AguardandoPagamento);
            await pedidoRepository.UpdateAsync(pedido, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);
            return pagamento;
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<(bool Sucesso, string TransacaoId, string PayloadEnvio, string PayloadRetorno)>
        ProcessarComProvedorAsync(
            Pagamento pagamento,
            Pedido pedido,
            string provedor,
            CancellationToken cancellationToken)
    {
        try
        {
            return await pagamentoMockService.ProcessarPagamentoMockAsync(pedido.Id, pedido.ValorTotal, provedor, cancellationToken);
        }
        catch (Exception exception)
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                await TratarFalhaProvedorAsync(pagamento, pedido, exception, cancellationToken);
                await unitOfWork.CommitAsync(cancellationToken);
            }
            catch
            {
                await unitOfWork.RollbackAsync(cancellationToken);
            }
            throw new DomainException("Falha na integração com o serviço de pagamentos. Tente novamente mais tarde.", exception);
        }
    }

    private async Task TratarFalhaProvedorAsync(Pagamento pagamento, Pedido pedido, Exception exception, CancellationToken cancellationToken)
    {
        pagamentoDomainService.MarcarComoErro(pagamento, exception.ToString());
        await pagamentoRepository.UpdateAsync(pagamento, cancellationToken);

        var auditoria = new Auditoria
        {
            UsuarioId = null,
            Entidade = "Pagamento",
            EntidadeId = pagamento.Id,
            Acao = "FALHA_INTEGRACAO_PAGAMENTO",
            DadosNovos = $"Falha na integração de pagamento do pedido {pedido.Id}. Erro: {exception.Message}",
            CriadoEm = DateTime.UtcNow
        };
        new AuditoriaValidator().ValidaOuLancaExcecao(auditoria);
        await auditoriaRepository.InsertAsync(auditoria, cancellationToken);
    }

    private async Task AtualizarStatusPosProcessamentoAsync(
        Pagamento pagamento,
        Pedido pedido,
        (bool Sucesso, string TransacaoId, string PayloadEnvio, string PayloadRetorno) result,
        string provedor,
        CancellationToken cancellationToken)
    {
        pagamentoDomainService.AtualizarAposProcessamento(
            pagamento, result.Sucesso, result.TransacaoId, result.PayloadEnvio, result.PayloadRetorno);
        await pagamentoRepository.UpdateAsync(pagamento, cancellationToken);

        if (result.Sucesso)
        {
            pedido.AlterarStatus(Domain.Enums.StatusPedido.PagamentoAprovado);
            await pedidoRepository.UpdateAsync(pedido, cancellationToken);

            if (pedido.ClienteId.HasValue)
            {
                try { await aplicarPontosFidelidadeUseCase.ExecutarAsync(pedido.Id, cancellationToken); } catch { }
            }
        }
        else
        {
            pedido.AlterarStatus(Domain.Enums.StatusPedido.PagamentoRecusado);
            await pedidoRepository.UpdateAsync(pedido, cancellationToken);
        }
    }

    private async Task RegistrarAuditoriaAsync(
        Pagamento pagamento,
        Pedido pedido,
        (bool Sucesso, string TransacaoId, string PayloadEnvio, string PayloadRetorno) result,
        string provedor,
        CancellationToken cancellationToken)
    {
        var auditoria = new Auditoria
        {
            UsuarioId = null,
            Entidade = "Pagamento",
            EntidadeId = pagamento.Id,
            Acao = "PROCESSAR_PAGAMENTO",
            DadosNovos = $"Provedor: {provedor}, Sucesso: {result.Sucesso}, TransaçãoId: {result.TransacaoId}. Status Pedido: {pedido.Status}",
            CriadoEm = DateTime.UtcNow
        };
        new AuditoriaValidator().ValidaOuLancaExcecao(auditoria);
        await auditoriaRepository.InsertAsync(auditoria, cancellationToken);
    }

    private static PagamentoResponse MapearParaResponse(Pagamento pagamento) => new()
    {
        Id = pagamento.Id,
        PedidoId = pagamento.PedidoId,
        Provedor = pagamento.Provedor,
        Status = pagamento.Status.ToString().ToUpper(),
        TransacaoExternaId = pagamento.TransacaoExternaId,
        SolicitadoEm = pagamento.SolicitadoEm,
        RespondidoEm = pagamento.RespondidoEm
    };
}
