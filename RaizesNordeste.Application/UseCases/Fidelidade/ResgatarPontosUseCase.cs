using RaizesNordeste.Application.Common;
using RaizesNordeste.Application.DTOs.Fidelidade;
using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Services;
using RaizesNordeste.Domain.Validators;

namespace RaizesNordeste.Application.UseCases.Fidelidade;

public class ResgatarPontosUseCase(
    IContaFidelidadeRepository contaRepository,
    IClienteRepository clienteRepository,
    IMovimentacaoPontosRepository movimentacaoRepository,
    IAuditoriaRepository auditoriaRepository,
    FidelidadeDomainService fidelidadeDomainService,
    IUnitOfWork unitOfWork)
{
    public async Task<ResultadoOperacao<ContaFidelidadeResponse>> ExecutarAsync(
        ResgatarPontosRequest request,
        UsuarioAutenticado? usuarioAutenticado = null,
        CancellationToken cancellationToken = default)
    {
        var titularidade = await ValidarTitularidadeAsync(request.ClienteId, usuarioAutenticado, cancellationToken);
        if (titularidade is not null)
        {
            return titularidade;
        }

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var conta = await contaRepository.ObterPorClienteIdAsync(request.ClienteId, cancellationToken);

            var saldoAnterior = conta?.SaldoPontos ?? 0;
            var movimentacao = fidelidadeDomainService.ResgatarPontos(conta!, request.Pontos, request.Descricao);

            new ContaFidelidadeValidator().ValidaOuLancaExcecao(conta!);
            await contaRepository.UpdateAsync(conta!, cancellationToken);

            new MovimentacaoPontosValidator().ValidaOuLancaExcecao(movimentacao);
            await movimentacaoRepository.InsertAsync(movimentacao, cancellationToken);

            var auditoria = new Auditoria
            {
                UsuarioId = null,
                Entidade = "ContaFidelidade",
                EntidadeId = conta!.Id,
                Acao = "RESGATE_PONTOS",
                DadosAnteriores = $"Saldo anterior: {saldoAnterior}",
                DadosNovos = $"Saldo novo: {conta.SaldoPontos}, Pontos resgatados: {request.Pontos}",
                CriadoEm = DateTime.UtcNow
            };
            new AuditoriaValidator().ValidaOuLancaExcecao(auditoria);
            await auditoriaRepository.InsertAsync(auditoria, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);

            return ResultadoOperacao<ContaFidelidadeResponse>.Sucesso(new ContaFidelidadeResponse
            {
                Id = conta.Id,
                ClienteId = conta.ClienteId,
                SaldoPontos = conta.SaldoPontos,
                Ativa = conta.Ativa,
                CriadoEm = conta.CriadoEm
            });
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<ResultadoOperacao<ContaFidelidadeResponse>?> ValidarTitularidadeAsync(
        int clienteId,
        UsuarioAutenticado? usuarioAutenticado,
        CancellationToken cancellationToken)
    {
        if (usuarioAutenticado?.EhCliente != true)
        {
            return null;
        }

        if (!usuarioAutenticado.UsuarioId.HasValue)
        {
            return ResultadoOperacao<ContaFidelidadeResponse>.Proibido(
                "Operacao nao permitida: cliente autenticado nao encontrado.",
                "CLIENTE_NAO_ENCONTRADO");
        }

        var cliente = await clienteRepository.ObterPorUsuarioIdAsync(usuarioAutenticado.UsuarioId.Value, cancellationToken);
        return cliente is not null && cliente.Id == clienteId
            ? null
            : ResultadoOperacao<ContaFidelidadeResponse>.Proibido(
                "Operacao nao permitida: o cliente autenticado so pode resgatar pontos para a sua propria conta.",
                "TITULARIDADE_INVALIDA");
    }
}
