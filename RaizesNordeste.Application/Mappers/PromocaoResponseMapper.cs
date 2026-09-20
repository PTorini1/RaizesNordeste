using RaizesNordeste.Application.DTOs.Promocoes;
using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases;

internal static class PromocaoResponseMapper
{
    public static async Task<IReadOnlyList<PromocaoResponse>> MapearListaAsync(
        IEnumerable<Promocao> promocoes,
        IProdutoRepository produtoRepository,
        CancellationToken cancellationToken)
    {
        var response = new List<PromocaoResponse>();

        foreach (var promocao in promocoes)
        {
            string? produtoNome = null;
            if (promocao.ProdutoId.HasValue)
            {
                var produto = await produtoRepository.GetByIdAsync(promocao.ProdutoId.Value, cancellationToken);
                produtoNome = produto?.Nome;
            }

            response.Add(new PromocaoResponse
            {
                Id = promocao.Id,
                Nome = promocao.Nome,
                Descricao = promocao.Descricao,
                TipoDesconto = promocao.TipoDesconto,
                ValorDesconto = promocao.ValorDesconto,
                InicioVigencia = promocao.InicioVigencia,
                FimVigencia = promocao.FimVigencia,
                Ativa = promocao.Ativa,
                ProdutoId = promocao.ProdutoId,
                ProdutoNome = produtoNome,
                CanalPedido = promocao.CanalPedido?.ToString().ToUpper(),
                PerfilCliente = promocao.PerfilCliente
            });
        }

        return response;
    }
}
