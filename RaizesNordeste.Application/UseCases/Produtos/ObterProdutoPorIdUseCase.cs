using RaizesNordeste.Application.DTOs.Produtos;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases.Produtos;

public class ObterProdutoPorIdUseCase(IProdutoRepository produtoRepository)
{
    public async Task<ProdutoResponse?> ExecutarAsync(int id, CancellationToken cancellationToken = default)
    {
        var produto = await produtoRepository.GetByIdAsync(id, cancellationToken);
        return produto is null ? null : ProdutoUseCaseMapper.Mapear(produto);
    }
}
