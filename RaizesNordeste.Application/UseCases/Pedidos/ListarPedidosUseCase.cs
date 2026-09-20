using RaizesNordeste.Application.DTOs.Pedidos;
using RaizesNordeste.Application.Mappers;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases.Pedidos;

public class ListarPedidosUseCase(IPedidoRepository pedidoRepository)
{
    public async Task<IReadOnlyList<PedidoResponse>> ExecutarAsync(CanalPedido? canalPedido, CancellationToken cancellationToken = default)
    {
        var pedidos = await pedidoRepository.ListAsync(
            predicate: canalPedido.HasValue ? p => p.CanalPedido == canalPedido.Value : null,
            cancellationToken: cancellationToken);

        var response = new List<PedidoResponse>();
        foreach (var pedido in pedidos)
        {
            var pedidoComItens = await pedidoRepository.ObterPorIdComItensAsync(pedido.Id, cancellationToken);
            if (pedidoComItens is not null)
            {
                response.Add(UtilPedidoMapper.MapearParaResponse(pedidoComItens));
            }
        }

        return response;
    }
}
