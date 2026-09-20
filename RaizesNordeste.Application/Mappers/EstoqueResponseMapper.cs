using RaizesNordeste.Application.DTOs.Estoque;
using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases;

internal static class EstoqueResponseMapper
{
    public static async Task<EstoqueResponse> MapearAsync(
        Estoque estoque,
        IProdutoRepository produtoRepository,
        CancellationToken cancellationToken)
    {
        var produto = await produtoRepository.GetByIdAsync(estoque.ProdutoId, cancellationToken);

        return new EstoqueResponse
        {
            Id = estoque.Id,
            UnidadeId = estoque.UnidadeId,
            ProdutoId = estoque.ProdutoId,
            ProdutoNome = produto?.Nome ?? $"Produto {estoque.ProdutoId}",
            QuantidadeAtual = estoque.QuantidadeAtual,
            QuantidadeMinima = estoque.QuantidadeMinima,
            AtualizadoEm = estoque.AtualizadoEm
        };
    }
}
