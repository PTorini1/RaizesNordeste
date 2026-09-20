namespace RaizesNordeste.Application.DTOs.Pagamentos;

public class PagamentoResponse
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public string Provedor { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? TransacaoExternaId { get; set; }
    public DateTime SolicitadoEm { get; set; }
    public DateTime? RespondidoEm { get; set; }
}
