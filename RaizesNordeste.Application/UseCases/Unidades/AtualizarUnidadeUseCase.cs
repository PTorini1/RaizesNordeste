using RaizesNordeste.Application.DTOs.Unidades;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Services;

namespace RaizesNordeste.Application.UseCases.Unidades;

public class AtualizarUnidadeUseCase(
    IUnidadeRepository unidadeRepository,
    UnidadeDomainService unidadeDomainService)
{
    public async Task<UnidadeResponse> ExecutarAsync(int id, AtualizarUnidadeRequest request, CancellationToken cancellationToken = default)
    {
        var unidade = await unidadeRepository.GetByIdAsync(id, cancellationToken);
        unidadeDomainService.ValidarExistente(unidade, id);

        unidadeDomainService.AtualizarUnidade(
            unidade!, request.Nome, request.Cidade, request.Estado, request.Endereco, request.TipoOperacao, request.Ativa);
        await unidadeRepository.UpdateAsync(unidade!, cancellationToken);

        return new UnidadeResponse
        {
            Id = unidade!.Id,
            Nome = unidade.Nome,
            Cidade = unidade.Cidade,
            Estado = unidade.Estado,
            Endereco = unidade.Endereco,
            TipoOperacao = unidade.TipoOperacao,
            Ativa = unidade.Ativa
        };
    }
}
