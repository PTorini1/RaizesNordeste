using RaizesNordeste.Application.DTOs.Produtos;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases.Produtos;

public class ListarProdutosUseCase(IProdutoRepository produtoRepository)
{
    public async Task<IReadOnlyList<ProdutoResponse>> ExecutarAsync(bool? apenasAtivos, string? categoria, CancellationToken cancellationToken = default)
    {
        var produtos = await produtoRepository.ListAsync(
            p => (!apenasAtivos.HasValue || p.Ativo == apenasAtivos.Value)
                && (string.IsNullOrWhiteSpace(categoria) || p.Categoria == categoria),
            orderBy: query => query.OrderBy(p => p.Nome),
            cancellationToken: cancellationToken);

        return produtos.Select(ProdutoUseCaseMapper.Mapear).ToList();
    }
}
