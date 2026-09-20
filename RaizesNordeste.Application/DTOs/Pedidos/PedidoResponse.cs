namespace RaizesNordeste.Application.DTOs.Pedidos;

public class PedidoResponse
{
    public int Id { get; set; }
    public int? ClienteId { get; set; }
    public int UnidadeId { get; set; }
    public string CanalPedido { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
    public List<ItemPedidoResponse> Itens { get; set; } = [];
}

public class ItemPedidoResponse
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public string ProdutoNome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
}
