using RaizesNordeste.Application.DTOs.Unidades;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases.Unidades;

public class ObterUnidadePorIdUseCase(IUnidadeRepository unidadeRepository)
{
    public async Task<UnidadeResponse?> ExecutarAsync(int id, CancellationToken cancellationToken = default)
    {
        var unidade = await unidadeRepository.GetByIdAsync(id, cancellationToken);
        return unidade is null ? null : UnidadeResponseMapper.Mapear(unidade);
    }
}
