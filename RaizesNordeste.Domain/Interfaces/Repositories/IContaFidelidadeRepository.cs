using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Interfaces.Repositories;

public interface IContaFidelidadeRepository : IGenericRepository<ContaFidelidade>
{
    Task<ContaFidelidade?> ObterPorClienteIdAsync(int clienteId, CancellationToken cancellationToken = default);
}
