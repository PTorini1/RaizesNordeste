using Microsoft.EntityFrameworkCore;
using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Infrastructure.Persistence;

namespace RaizesNordeste.Infrastructure.Repositories;

public class ConsentimentoLGPDRepository(RaizesNordesteDbContext dbContext)
    : EfRepository<ConsentimentoLGPD>(dbContext), IConsentimentoLGPDRepository
{
    private readonly RaizesNordesteDbContext _dbContext = dbContext;

    public async Task<IReadOnlyList<ConsentimentoLGPD>> ListarPorClienteIdAsync(int clienteId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ConsentimentoLgpd
            .Where(c => c.ClienteId == clienteId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> PossuiConsentimentoAtivoAsync(int clienteId, string finalidade, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ConsentimentoLgpd
            .AnyAsync(c => c.ClienteId == clienteId
                           && c.Finalidade == finalidade
                           && c.Consentido
                           && (c.RevogadoEm == null),
                      cancellationToken);
    }
}
