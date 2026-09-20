using Microsoft.EntityFrameworkCore;
using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Infrastructure.Persistence;

namespace RaizesNordeste.Infrastructure.Repositories;

public class CardapioUnidadeRepository(RaizesNordesteDbContext dbContext)
    : EfRepository<CardapioUnidade>(dbContext), ICardapioUnidadeRepository
{
    private readonly RaizesNordesteDbContext _dbContext = dbContext;

    public async Task<IReadOnlyList<CardapioUnidade>> ListarPorUnidadeAsync(int unidadeId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.CardapioUnidade
            .Include(c => c.Produto)
            .Where(c => c.UnidadeId == unidadeId && c.Disponivel)
            .ToListAsync(cancellationToken);
    }
}
