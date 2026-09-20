using RaizesNordeste.Application.DTOs.Produtos;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Application.UseCases;

internal static class ProdutoUseCaseMapper
{
    public static ProdutoResponse Mapear(Produto produto)
    {
        return new ProdutoResponse
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            PrecoBase = produto.PrecoBase,
            Categoria = produto.Categoria,
            Sazonal = produto.Sazonal,
            Ativo = produto.Ativo
        };
    }
}
