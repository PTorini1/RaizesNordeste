using RaizesNordeste.Application.DTOs.Produtos;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Services;

namespace RaizesNordeste.Application.UseCases.Produtos;

public class AtualizarProdutoUseCase(
    IProdutoRepository produtoRepository,
    ProdutoDomainService produtoDomainService)
{
    public async Task<ProdutoResponse?> ExecutarAsync(int id, AtualizarProdutoRequest request, CancellationToken cancellationToken = default)
    {
        var produto = await produtoRepository.GetByIdAsync(id, cancellationToken);
        if (produto is null) return null;

        produtoDomainService.AtualizarProduto(
            produto, request.Nome, request.Descricao, request.PrecoBase, request.Categoria, request.Sazonal, request.Ativo);

        await produtoRepository.UpdateAsync(produto, cancellationToken);
        return ProdutoUseCaseMapper.Mapear(produto);
    }
}
