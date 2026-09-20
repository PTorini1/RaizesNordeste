using RaizesNordeste.Application.DTOs.Promocoes;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases.Promocoes;

public class ListarPromocoesUseCase(
    IPromocaoRepository promocaoRepository,
    IProdutoRepository produtoRepository)
{
    public async Task<IReadOnlyList<PromocaoResponse>> ExecutarAsync(CancellationToken cancellationToken = default)
    {
        var promocoes = await promocaoRepository.ListAsync(cancellationToken: cancellationToken);
        return await PromocaoResponseMapper.MapearListaAsync(promocoes, produtoRepository, cancellationToken);
    }
}
