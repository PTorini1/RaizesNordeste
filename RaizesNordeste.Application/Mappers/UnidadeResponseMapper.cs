using RaizesNordeste.Application.DTOs.Unidades;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Application.UseCases;

internal static class UnidadeResponseMapper
{
    public static UnidadeResponse Mapear(Unidade unidade)
    {
        return new UnidadeResponse
        {
            Id = unidade.Id,
            Nome = unidade.Nome,
            Cidade = unidade.Cidade,
            Estado = unidade.Estado,
            Endereco = unidade.Endereco,
            TipoOperacao = unidade.TipoOperacao,
            Ativa = unidade.Ativa
        };
    }
}
