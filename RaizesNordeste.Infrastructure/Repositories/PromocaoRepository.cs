using Microsoft.EntityFrameworkCore;
using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Infrastructure.Persistence;

namespace RaizesNordeste.Infrastructure.Repositories;

public class PromocaoRepository(RaizesNordesteDbContext dbContext)
    : EfRepository<Promocao>(dbContext), IPromocaoRepository
{
    private readonly RaizesNordesteDbContext _dbContext = dbContext;

    public async Task<IReadOnlyList<Promocao>> ListarAtivasVigentesAsync(DateTime data, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Promocao
            .Where(p => p.Ativa && p.InicioVigencia <= data && p.FimVigencia >= data)
            .ToListAsync(cancellationToken);
    }
}
