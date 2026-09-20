using RaizesNordeste.Application.DTOs.Cardapio;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Interfaces.Services;

namespace RaizesNordeste.Application.UseCases.Cardapio;

public class ConsultarCardapioUnidadeUseCase(
    ICardapioUnidadeRepository cardapioRepository,
    IRedisCacheService redisCacheService)
{
    public async Task<List<CardapioResponse>> ExecutarAsync(int unidadeId, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"cardapio:unidade:{unidadeId}";

        try
        {
            var cachedCardapio = await redisCacheService.GetAsync<List<CardapioResponse>>(cacheKey, cancellationToken);
            if (cachedCardapio != null)
            {
                return cachedCardapio;
            }
        }
        catch
        {
        }

        var cardapioBanco = await cardapioRepository.ListarPorUnidadeAsync(unidadeId, cancellationToken);

        var response = cardapioBanco.Select(c => new CardapioResponse
        {
            ProdutoId = c.ProdutoId,
            ProdutoNome = c.Produto?.Nome ?? $"Produto {c.ProdutoId}",
            Descricao = c.Produto?.Descricao,
            Preco = c.Preco,
            Categoria = c.Produto?.Categoria ?? "Geral",
            Sazonal = c.Produto?.Sazonal ?? false,
            Disponivel = c.Disponivel
        }).ToList();

        if (response.Count > 0)
        {
            try
            {
                await redisCacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(5), cancellationToken);
            }
            catch
            {
            }
        }

        return response;
    }
}
