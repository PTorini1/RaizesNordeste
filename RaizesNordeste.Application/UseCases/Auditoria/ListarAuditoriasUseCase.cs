using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases.Auditorias;

public class ListarAuditoriasUseCase(IAuditoriaRepository auditoriaRepository)
{
    public async Task<IReadOnlyList<Auditoria>> ExecutarAsync(string? entidade, string? acao, CancellationToken cancellationToken = default)
    {
        return await auditoriaRepository.ListAsync(
            predicate: a => (string.IsNullOrWhiteSpace(entidade) || a.Entidade.ToLower() == entidade.ToLower()) &&
                            (string.IsNullOrWhiteSpace(acao) || a.Acao.ToLower() == acao.ToLower()),
            orderBy: q => q.OrderByDescending(a => a.CriadoEm),
            cancellationToken: cancellationToken);
    }
}
