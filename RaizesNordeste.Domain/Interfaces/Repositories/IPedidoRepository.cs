using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Interfaces.Repositories;

public interface IPedidoRepository : IGenericRepository<Pedido>
{
    Task<Pedido?> ObterPorIdComItensAsync(int id, CancellationToken cancellationToken = default);
}
