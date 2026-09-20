using RaizesNordeste.Application.DTOs.Produtos;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Services;

namespace RaizesNordeste.Application.UseCases.Produtos;

public class CriarProdutoUseCase(
    IProdutoRepository produtoRepository,
    ProdutoDomainService produtoDomainService)
{
    public async Task<ProdutoResponse> ExecutarAsync(CriarProdutoRequest request, CancellationToken cancellationToken = default)
    {
        var produto = produtoDomainService.CriarProduto(
            request.Nome, request.Descricao, request.PrecoBase, request.Categoria, request.Sazonal, request.Ativo);

        var salvo = await produtoRepository.InsertAsync(produto, cancellationToken);
        return ProdutoUseCaseMapper.Mapear(salvo);
    }
}
