namespace RaizesNordeste.Application.DTOs.Pagamentos;

public class SolicitarPagamentoRequest
{
    public int PedidoId { get; set; }
    public string Provedor { get; set; } = string.Empty;
    public string ChaveIdempotencia { get; set; } = string.Empty;
}
