using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases.Auditorias;

public class ListarAuditoriasPorEntidadeUseCase(IAuditoriaRepository auditoriaRepository)
{
    public async Task<IReadOnlyList<Auditoria>> ExecutarAsync(string entidade, CancellationToken cancellationToken = default)
    {
        return await auditoriaRepository.ListAsync(
            predicate: a => a.Entidade.ToLower() == entidade.ToLower(),
            orderBy: q => q.OrderByDescending(a => a.CriadoEm),
            cancellationToken: cancellationToken);
    }
}
