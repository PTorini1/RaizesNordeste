using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Services;

namespace RaizesNordeste.Application.UseCases.Unidades;

public class DesativarUnidadeUseCase(
    IUnidadeRepository unidadeRepository,
    UnidadeDomainService unidadeDomainService)
{
    public async Task ExecutarAsync(int id, CancellationToken cancellationToken = default)
    {
        var unidade = await unidadeRepository.GetByIdAsync(id, cancellationToken);
        unidadeDomainService.ValidarExistente(unidade, id);

        unidadeDomainService.Desativar(unidade!);
        await unidadeRepository.UpdateAsync(unidade!, cancellationToken);
    }
}
