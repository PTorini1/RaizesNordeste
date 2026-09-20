using RaizesNordeste.Application.DTOs.Promocoes;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases.Promocoes;

public class ListarPromocoesAtivasUseCase(
    IPromocaoRepository promocaoRepository,
    IProdutoRepository produtoRepository)
{
    public async Task<IReadOnlyList<PromocaoResponse>> ExecutarAsync(
        int? produtoId,
        CanalPedido? canalPedido,
        string? perfilCliente,
        CancellationToken cancellationToken = default)
    {
        var promocoes = await promocaoRepository.ListarAtivasVigentesAsync(DateTime.UtcNow, cancellationToken);
        var query = promocoes.AsEnumerable();

        if (produtoId.HasValue)
        {
            query = query.Where(p => p.ProdutoId == produtoId.Value || p.ProdutoId == null);
        }

        if (canalPedido.HasValue)
        {
            query = query.Where(p => p.CanalPedido == canalPedido.Value || p.CanalPedido == null);
        }

        if (!string.IsNullOrWhiteSpace(perfilCliente))
        {
            query = query.Where(p => string.Equals(p.PerfilCliente, perfilCliente, StringComparison.OrdinalIgnoreCase) || p.PerfilCliente == null);
        }

        return await PromocaoResponseMapper.MapearListaAsync(query, produtoRepository, cancellationToken);
    }
}
