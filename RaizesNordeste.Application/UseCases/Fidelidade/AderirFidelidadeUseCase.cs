using RaizesNordeste.Application.Common;
using RaizesNordeste.Application.DTOs.Fidelidade;
using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Services;
using RaizesNordeste.Domain.Validators;

namespace RaizesNordeste.Application.UseCases.Fidelidade;

public class AderirFidelidadeUseCase(
    IClienteRepository clienteRepository,
    IConsentimentoLGPDRepository consentimentoRepository,
    IContaFidelidadeRepository contaRepository,
    IAuditoriaRepository auditoriaRepository,
    FidelidadeDomainService fidelidadeDomainService,
    IUnitOfWork unitOfWork)
{
    public async Task<ResultadoOperacao<ContaFidelidadeResponse>> ExecutarAsync(
        AderirFidelidadeRequest request,
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
            var cliente = await clienteRepository.GetByIdAsync(request.ClienteId, cancellationToken);
            fidelidadeDomainService.ValidarClientePodeAderir(cliente);

            var consentimento = new ConsentimentoLGPD
            {
                ClienteId = request.ClienteId,
                Finalidade = request.Finalidade,
                Consentido = request.Consentido,
                VersaoTermo = request.VersaoTermo,
                ConcedidoEm = request.Consentido ? DateTime.UtcNow : null,
                RevogadoEm = request.Consentido ? null : DateTime.UtcNow
            };
            new ConsentimentoLGPDValidator().ValidaOuLancaExcecao(consentimento);
            await consentimentoRepository.InsertAsync(consentimento, cancellationToken);

            var contaExistente = await contaRepository.ObterPorClienteIdAsync(request.ClienteId, cancellationToken);
            var conta = fidelidadeDomainService.CriarOuAtualizarConta(request.ClienteId, contaExistente, request.Consentido);

            if (contaExistente is null)
            {
                new ContaFidelidadeValidator().ValidaOuLancaExcecao(conta);
                conta = await contaRepository.InsertAsync(conta, cancellationToken);
            }
            else
            {
                await contaRepository.UpdateAsync(conta, cancellationToken);
            }

            var auditoria = new Auditoria
            {
                UsuarioId = null,
                Entidade = "ContaFidelidade",
                EntidadeId = conta.Id,
                Acao = request.Consentido ? "ADERIR_FIDELIDADE" : "REVOGAR_FIDELIDADE",
                DadosNovos = $"Consentimento LGPD Finalidade {request.Finalidade} definido para {request.Consentido}",
                CriadoEm = DateTime.UtcNow
            };
            new AuditoriaValidator().ValidaOuLancaExcecao(auditoria);
            await auditoriaRepository.InsertAsync(auditoria, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);

            return ResultadoOperacao<ContaFidelidadeResponse>.Criado(new ContaFidelidadeResponse
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
                "Operacao nao permitida: o cliente autenticado so pode aderir ao programa de fidelidade para si mesmo.",
                "TITULARIDADE_INVALIDA");
    }
}
