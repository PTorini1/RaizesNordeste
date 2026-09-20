using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Interfaces.Repositories;

public interface ICardapioUnidadeRepository : IGenericRepository<CardapioUnidade>
{
    Task<IReadOnlyList<CardapioUnidade>> ListarPorUnidadeAsync(int unidadeId, CancellationToken cancellationToken = default);
}
