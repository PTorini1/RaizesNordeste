using RaizesNordeste.Application.DTOs.Cardapio;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Interfaces.Services;
using RaizesNordeste.Domain.Services;

namespace RaizesNordeste.Application.UseCases.Cardapio;

public class AssociarProdutoCardapioUseCase(
    ICardapioUnidadeRepository cardapioRepository,
    IUnidadeRepository unidadeRepository,
    IProdutoRepository produtoRepository,
    IRedisCacheService redisCacheService,
    CardapioDomainService cardapioDomainService)
{
    public async Task<CardapioResponse> ExecutarAsync(int unidadeId, AssociarProdutoRequest request, CancellationToken cancellationToken = default)
    {
        var unidade = await unidadeRepository.GetByIdAsync(unidadeId, cancellationToken);
        var produto = await produtoRepository.GetByIdAsync(request.ProdutoId, cancellationToken);
        var cardapioExistente = await cardapioRepository.ListarPorUnidadeAsync(unidadeId, cancellationToken);

        cardapioDomainService.ValidarAssociacao(unidade, produto, cardapioExistente, request.ProdutoId);

        var cardapio = cardapioDomainService.CriarAssociacao(
            unidadeId, request.ProdutoId, request.Preco, request.Disponivel,
            request.InicioVigencia, request.FimVigencia);

        var salvo = await cardapioRepository.InsertAsync(cardapio, cancellationToken);

        try { await redisCacheService.RemoveAsync($"cardapio:unidade:{unidadeId}", cancellationToken); } catch { }

        return new CardapioResponse
        {
            ProdutoId = salvo.ProdutoId,
            ProdutoNome = produto!.Nome,
            Descricao = produto.Descricao,
            Preco = salvo.Preco,
            Categoria = produto.Categoria,
            Sazonal = produto.Sazonal,
            Disponivel = salvo.Disponivel
        };
    }
}
