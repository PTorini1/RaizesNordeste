using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Interfaces.Repositories;

public interface IPagamentoRepository : IGenericRepository<Pagamento>
{
    Task<Pagamento?> ObterPorPedidoIdAsync(int pedidoId, CancellationToken cancellationToken = default);
    Task<Pagamento?> ObterPorChaveIdempotenciaAsync(string chaveIdempotencia, CancellationToken cancellationToken = default);
}
