using Microsoft.EntityFrameworkCore;
using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Infrastructure.Persistence;

namespace RaizesNordeste.Infrastructure.Repositories;

public class PagamentoRepository(RaizesNordesteDbContext dbContext)
    : EfRepository<Pagamento>(dbContext), IPagamentoRepository
{
    private readonly RaizesNordesteDbContext _dbContext = dbContext;

    public async Task<Pagamento?> ObterPorPedidoIdAsync(int pedidoId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Pagamento
            .FirstOrDefaultAsync(p => p.PedidoId == pedidoId, cancellationToken);
    }

    public async Task<Pagamento?> ObterPorChaveIdempotenciaAsync(string chaveIdempotencia, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Pagamento
            .FirstOrDefaultAsync(p => p.ChaveIdempotencia == chaveIdempotencia, cancellationToken);
    }
}
