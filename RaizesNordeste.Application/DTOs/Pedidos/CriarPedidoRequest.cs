using RaizesNordeste.Domain.Enums;

namespace RaizesNordeste.Application.DTOs.Pedidos;

public class CriarPedidoRequest
{
    public int? ClienteId { get; set; }
    public int UnidadeId { get; set; }
    public CanalPedido CanalPedido { get; set; }
    public List<CriarItemPedidoRequest> Itens { get; set; } = [];
}

public class CriarItemPedidoRequest
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
}
