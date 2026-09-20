using RaizesNordeste.Application.DTOs.Pedidos;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Application.Mappers;

public static class UtilPedidoMapper
{
    public static PedidoResponse MapearParaResponse(Pedido pedido)
    {
        return new PedidoResponse
        {
            Id = pedido.Id,
            ClienteId = pedido.ClienteId,
            UnidadeId = pedido.UnidadeId,
            CanalPedido = pedido.CanalPedido.ToString().ToUpper(),
            Status = pedido.Status.ToString().ToUpper(),
            ValorTotal = pedido.ValorTotal,
            CriadoEm = pedido.CriadoEm,
            AtualizadoEm = pedido.AtualizadoEm,
            Itens = pedido.Itens.Select(i => new ItemPedidoResponse
            {
                Id = i.Id,
                ProdutoId = i.ProdutoId,
                ProdutoNome = i.Produto?.Nome ?? $"Produto {i.ProdutoId}",
                Quantidade = i.Quantidade,
                ValorUnitario = i.ValorUnitario,
                ValorTotal = i.ValorTotal
            }).ToList()
        };
    }
}
