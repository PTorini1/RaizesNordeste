using RaizesNordeste.Domain.Enums;

namespace RaizesNordeste.Domain.Entities;

public class Pagamento
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public string Provedor { get; set; } = string.Empty;
    public StatusPagamento Status { get; set; } = StatusPagamento.Pendente;
    public string? TransacaoExternaId { get; set; }
    public string? PayloadEnvio { get; set; }
    public string? PayloadRetorno { get; set; }
    public string ChaveIdempotencia { get; set; } = string.Empty;
    public DateTime SolicitadoEm { get; set; } = DateTime.UtcNow;
    public DateTime? RespondidoEm { get; set; }

    public Pedido? Pedido { get; set; }
}
