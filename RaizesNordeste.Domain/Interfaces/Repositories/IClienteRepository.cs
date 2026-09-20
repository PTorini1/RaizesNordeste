using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Interfaces.Repositories;

public interface IClienteRepository : IGenericRepository<Cliente>
{
    Task<Cliente?> ObterPorUsuarioIdAsync(int usuarioId, CancellationToken cancellationToken = default);
}
