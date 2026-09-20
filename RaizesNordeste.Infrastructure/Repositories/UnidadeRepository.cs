using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Infrastructure.Persistence;

namespace RaizesNordeste.Infrastructure.Repositories;

public class UnidadeRepository(RaizesNordesteDbContext dbContext)
    : EfRepository<Unidade>(dbContext), IUnidadeRepository
{
}
