using RaizesNordeste.Application.DTOs.Cardapio;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Interfaces.Services;
using RaizesNordeste.Domain.Services;

namespace RaizesNordeste.Application.UseCases.Cardapio;

public class AtualizarProdutoCardapioUseCase(
    ICardapioUnidadeRepository cardapioRepository,
    IProdutoRepository produtoRepository,
    IRedisCacheService redisCacheService,
    CardapioDomainService cardapioDomainService)
{
    public async Task<CardapioResponse> ExecutarAsync(
        int unidadeId,
        int produtoId,
        AtualizarProdutoCardapioRequest request,
        CancellationToken cancellationToken = default)
    {
        var cardapioItens = await cardapioRepository.ListarPorUnidadeAsync(unidadeId, cancellationToken);
        var cardapio = cardapioDomainService.ValidarItemExistente(cardapioItens, produtoId, unidadeId);

        var produto = await produtoRepository.GetByIdAsync(produtoId, cancellationToken);
        _ = produto ?? throw new Domain.Exceptions.DomainException($"Produto ID {produtoId} não encontrado.");

        cardapioDomainService.AtualizarAssociacao(cardapio, request.Preco, request.Disponivel, request.InicioVigencia, request.FimVigencia);
        await cardapioRepository.UpdateAsync(cardapio, cancellationToken);

        try { await redisCacheService.RemoveAsync($"cardapio:unidade:{unidadeId}", cancellationToken); } catch { }

        return new CardapioResponse
        {
            ProdutoId = cardapio.ProdutoId,
            ProdutoNome = produto.Nome,
            Descricao = produto.Descricao,
            Preco = cardapio.Preco,
            Categoria = produto.Categoria,
            Sazonal = produto.Sazonal,
            Disponivel = cardapio.Disponivel
        };
    }
}
