using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Infrastructure.Persistence;

namespace RaizesNordeste.Infrastructure.Repositories;

public class MovimentacaoEstoqueRepository(RaizesNordesteDbContext dbContext)
    : EfRepository<MovimentacaoEstoque>(dbContext), IMovimentacaoEstoqueRepository
{
}
