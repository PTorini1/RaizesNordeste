using RaizesNordeste.Application.DTOs.Unidades;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Services;

namespace RaizesNordeste.Application.UseCases.Unidades;

public class CriarUnidadeUseCase(
    IUnidadeRepository unidadeRepository,
    UnidadeDomainService unidadeDomainService)
{
    public async Task<UnidadeResponse> ExecutarAsync(CriarUnidadeRequest request, CancellationToken cancellationToken = default)
    {
        var unidade = unidadeDomainService.CriarUnidade(
            request.Nome, request.Cidade, request.Estado, request.Endereco, request.TipoOperacao);
        var salvo = await unidadeRepository.InsertAsync(unidade, cancellationToken);

        return new UnidadeResponse
        {
            Id = salvo.Id,
            Nome = salvo.Nome,
            Cidade = salvo.Cidade,
            Estado = salvo.Estado,
            Endereco = salvo.Endereco,
            TipoOperacao = salvo.TipoOperacao,
            Ativa = salvo.Ativa
        };
    }
}
