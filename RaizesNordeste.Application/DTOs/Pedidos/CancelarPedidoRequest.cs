namespace RaizesNordeste.Application.DTOs.Pedidos;

public class CancelarPedidoRequest
{
    public int UsuarioId { get; set; }
    public string Motivo { get; set; } = string.Empty;
}
