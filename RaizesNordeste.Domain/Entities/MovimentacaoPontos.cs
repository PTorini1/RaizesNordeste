using RaizesNordeste.Domain.Enums;

namespace RaizesNordeste.Domain.Entities;

public class MovimentacaoPontos
{
    public int Id { get; set; }
    public int ContaFidelidadeId { get; set; }
    public int? PedidoId { get; set; }
    public TipoMovimentacaoPontos TipoMovimentacao { get; set; }
    public int Pontos { get; set; }
    public string? Descricao { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public ContaFidelidade? ContaFidelidade { get; set; }
    public Pedido? Pedido { get; set; }
}
