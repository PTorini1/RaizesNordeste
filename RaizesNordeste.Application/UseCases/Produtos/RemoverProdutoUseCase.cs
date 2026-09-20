using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases.Produtos;

public class RemoverProdutoUseCase(IProdutoRepository produtoRepository)
{
    public async Task<bool> ExecutarAsync(int id, CancellationToken cancellationToken = default)
    {
        return await produtoRepository.DeleteByIdAsync(id, cancellationToken);
    }
}
