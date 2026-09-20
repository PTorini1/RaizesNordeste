using Microsoft.EntityFrameworkCore;
using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Infrastructure.Persistence;

namespace RaizesNordeste.Infrastructure.Repositories;

public class ContaFidelidadeRepository(RaizesNordesteDbContext dbContext)
    : EfRepository<ContaFidelidade>(dbContext), IContaFidelidadeRepository
{
    private readonly RaizesNordesteDbContext _dbContext = dbContext;

    public async Task<ContaFidelidade?> ObterPorClienteIdAsync(int clienteId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ContaFidelidade
            .FirstOrDefaultAsync(c => c.ClienteId == clienteId, cancellationToken);
    }
}
