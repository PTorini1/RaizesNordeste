using Microsoft.EntityFrameworkCore;
using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Infrastructure.Persistence;

namespace RaizesNordeste.Infrastructure.Repositories;

public class ClienteRepository(RaizesNordesteDbContext dbContext)
    : EfRepository<Cliente>(dbContext), IClienteRepository
{
    private readonly RaizesNordesteDbContext _dbContext = dbContext;

    public async Task<Cliente?> ObterPorUsuarioIdAsync(int usuarioId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Cliente
            .Include(c => c.Usuario)
            .Include(c => c.ContaFidelidade)
            .Include(c => c.ConsentimentoLgpd)
            .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId, cancellationToken);
    }
}
