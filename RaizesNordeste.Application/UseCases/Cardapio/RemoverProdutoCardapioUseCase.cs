using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Interfaces.Services;
using RaizesNordeste.Domain.Services;

namespace RaizesNordeste.Application.UseCases.Cardapio;

public class RemoverProdutoCardapioUseCase(
    ICardapioUnidadeRepository cardapioRepository,
    IRedisCacheService redisCacheService,
    CardapioDomainService cardapioDomainService)
{
    public async Task ExecutarAsync(int unidadeId, int produtoId, CancellationToken cancellationToken = default)
    {
        var cardapioItens = await cardapioRepository.ListarPorUnidadeAsync(unidadeId, cancellationToken);
        var cardapio = cardapioDomainService.ValidarItemExistente(cardapioItens, produtoId, unidadeId);

        await cardapioRepository.DeleteAsync(cardapio, cancellationToken);

        try { await redisCacheService.RemoveAsync($"cardapio:unidade:{unidadeId}", cancellationToken); } catch { }
    }
}
