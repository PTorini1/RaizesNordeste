using RaizesNordeste.Application.DTOs.Unidades;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases.Unidades;

public class ListarUnidadesUseCase(IUnidadeRepository unidadeRepository)
{
    public async Task<IReadOnlyList<UnidadeResponse>> ExecutarAsync(bool? apenasAtivas, CancellationToken cancellationToken = default)
    {
        var unidades = await unidadeRepository.ListAsync(
            predicate: apenasAtivas.HasValue ? u => u.Ativa == apenasAtivas.Value : null,
            cancellationToken: cancellationToken);

        return unidades.Select(UnidadeResponseMapper.Mapear).ToList();
    }
}
