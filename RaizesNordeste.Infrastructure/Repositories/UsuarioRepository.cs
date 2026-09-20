using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Infrastructure.Persistence;

namespace RaizesNordeste.Infrastructure.Repositories;

public class UsuarioRepository(RaizesNordesteDbContext dbContext)
    : EfRepository<Usuario>(dbContext), IUsuarioRepository
{
}
