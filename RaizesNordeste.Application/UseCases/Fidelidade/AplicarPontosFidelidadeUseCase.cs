using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Services;
using RaizesNordeste.Domain.Validators;

namespace RaizesNordeste.Application.UseCases.Fidelidade;

public class AplicarPontosFidelidadeUseCase(
    IPedidoRepository pedidoRepository,
    IContaFidelidadeRepository contaFidelidadeRepository,
    IMovimentacaoPontosRepository movimentacaoPontosRepository,
    IConsentimentoLGPDRepository consentimentoLgpdRepository,
    IAuditoriaRepository auditoriaRepository,
    FidelidadeDomainService fidelidadeDomainService,
    IUnitOfWork unitOfWork)
{
    public async Task ExecutarAsync(int pedidoId, CancellationToken cancellationToken = default)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var pedido = await ObterPedidoComClienteAsync(pedidoId, cancellationToken);
            if (pedido is null)
            {
                await unitOfWork.CommitAsync(cancellationToken);
                return;
            }

            var clienteId = pedido.ClienteId!.Value;

            var consentimentoValido = await ValidarConsentimentoLgpdAsync(clienteId, pedido, cancellationToken);
            if (!consentimentoValido)
            {
                await unitOfWork.CommitAsync(cancellationToken);
                return;
            }

            var (conta, saldoAnterior) = await ObterOuCriarContaAsync(clienteId, cancellationToken);

            var movimentacao = fidelidadeDomainService.AcumularPontos(conta, pedido);
            if (movimentacao is null)
            {
                await unitOfWork.CommitAsync(cancellationToken);
                return;
            }

            await PersistirAcumuloAsync(conta, movimentacao, cancellationToken);
            await RegistrarAuditoriaAsync(conta, movimentacao, saldoAnterior, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<Pedido?> ObterPedidoComClienteAsync(int pedidoId, CancellationToken cancellationToken)
    {
        var pedido = await pedidoRepository.ObterPorIdComItensAsync(pedidoId, cancellationToken);
        _ = pedido ?? throw new DomainException($"Pedido ID {pedidoId} não encontrado.");

        return pedido.ClienteId.HasValue ? pedido : null;
    }

    private async Task<bool> ValidarConsentimentoLgpdAsync(int clienteId, Pedido pedido, CancellationToken cancellationToken)
    {
        var possuiConsentimento = await consentimentoLgpdRepository.PossuiConsentimentoAtivoAsync(
            clienteId, "FIDELIDADE", cancellationToken);

        if (!possuiConsentimento)
        {
            var auditoria = new Auditoria
            {
                UsuarioId = null,
                Entidade = "ContaFidelidade",
                EntidadeId = clienteId,
                Acao = "PONTOS_NAO_ACUMULADOS_LGPD",
                DadosNovos = $"Pontos do pedido {pedido.Id} não acumulados: Cliente não possui consentimento ativo para finalidade 'FIDELIDADE'.",
                CriadoEm = DateTime.UtcNow
            };
            new AuditoriaValidator().ValidaOuLancaExcecao(auditoria);
            await auditoriaRepository.InsertAsync(auditoria, cancellationToken);
        }

        return possuiConsentimento;
    }

    private async Task<(ContaFidelidade Conta, int SaldoAnterior)> ObterOuCriarContaAsync(int clienteId, CancellationToken cancellationToken)
    {
        var conta = await contaFidelidadeRepository.ObterPorClienteIdAsync(clienteId, cancellationToken);
        conta = fidelidadeDomainService.CriarOuAtualizarConta(clienteId, conta, true);

        if (conta.Id == 0)
        {
            new ContaFidelidadeValidator().ValidaOuLancaExcecao(conta);
            conta = await contaFidelidadeRepository.InsertAsync(conta, cancellationToken);
        }

        return (conta, conta.SaldoPontos);
    }

    private async Task PersistirAcumuloAsync(ContaFidelidade conta, MovimentacaoPontos movimentacao, CancellationToken cancellationToken)
    {
        new ContaFidelidadeValidator().ValidaOuLancaExcecao(conta);
        await contaFidelidadeRepository.UpdateAsync(conta, cancellationToken);

        new MovimentacaoPontosValidator().ValidaOuLancaExcecao(movimentacao);
        await movimentacaoPontosRepository.InsertAsync(movimentacao, cancellationToken);
    }

    private async Task RegistrarAuditoriaAsync(ContaFidelidade conta, MovimentacaoPontos movimentacao, int saldoAnterior, CancellationToken cancellationToken)
    {
        var auditoria = new Auditoria
        {
            UsuarioId = null,
            Entidade = "ContaFidelidade",
            EntidadeId = conta.Id,
            Acao = "ACUMULAR_PONTOS",
            DadosAnteriores = $"Saldo anterior: {saldoAnterior}",
            DadosNovos = $"Saldo novo: {conta.SaldoPontos}, Pontos acumulados: {movimentacao.Pontos}",
            CriadoEm = DateTime.UtcNow
        };
        new AuditoriaValidator().ValidaOuLancaExcecao(auditoria);
        await auditoriaRepository.InsertAsync(auditoria, cancellationToken);
    }
}
