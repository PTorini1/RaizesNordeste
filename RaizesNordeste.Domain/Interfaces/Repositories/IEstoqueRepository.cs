using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Interfaces.Repositories;

public interface IEstoqueRepository : IGenericRepository<Estoque>
{
    Task<Estoque?> ObterPorUnidadeEProdutoAsync(int unidadeId, int produtoId, CancellationToken cancellationToken = default);
}
