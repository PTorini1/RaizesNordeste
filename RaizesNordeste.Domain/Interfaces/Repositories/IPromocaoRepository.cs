using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Interfaces.Repositories;

public interface IPromocaoRepository : IGenericRepository<Promocao>
{
    Task<IReadOnlyList<Promocao>> ListarAtivasVigentesAsync(DateTime data, CancellationToken cancellationToken = default);
}
