using Microsoft.EntityFrameworkCore;
using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Infrastructure.Persistence;

namespace RaizesNordeste.Infrastructure.Repositories;

public class EstoqueRepository(RaizesNordesteDbContext dbContext)
    : EfRepository<Estoque>(dbContext), IEstoqueRepository
{
    private readonly RaizesNordesteDbContext _dbContext = dbContext;

    public async Task<Estoque?> ObterPorUnidadeEProdutoAsync(int unidadeId, int produtoId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Estoque
            .Include(e => e.Produto)
            .FirstOrDefaultAsync(e => e.UnidadeId == unidadeId && e.ProdutoId == produtoId, cancellationToken);
    }
}
