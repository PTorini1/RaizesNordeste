using Microsoft.EntityFrameworkCore;
using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Infrastructure.Persistence;

namespace RaizesNordeste.Infrastructure.Repositories;

public class PedidoRepository(RaizesNordesteDbContext dbContext)
    : EfRepository<Pedido>(dbContext), IPedidoRepository
{
    private readonly RaizesNordesteDbContext _dbContext = dbContext;

    public async Task<Pedido?> ObterPorIdComItensAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Pedido
            .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
            .Include(p => p.Pagamento)
            .Include(p => p.Cliente)
            .Include(p => p.Unidade)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}
