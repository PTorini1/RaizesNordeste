using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Interfaces.Repositories;

public interface IConsentimentoLGPDRepository : IGenericRepository<ConsentimentoLGPD>
{
    Task<IReadOnlyList<ConsentimentoLGPD>> ListarPorClienteIdAsync(int clienteId, CancellationToken cancellationToken = default);
    Task<bool> PossuiConsentimentoAtivoAsync(int clienteId, string finalidade, CancellationToken cancellationToken = default);
}
