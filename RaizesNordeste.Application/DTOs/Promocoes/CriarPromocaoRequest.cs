using RaizesNordeste.Domain.Enums;

namespace RaizesNordeste.Application.DTOs.Promocoes;

public class CriarPromocaoRequest
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string TipoDesconto { get; set; } = string.Empty;
    public decimal ValorDesconto { get; set; }
    public DateTime InicioVigencia { get; set; }
    public DateTime FimVigencia { get; set; }
    public int? ProdutoId { get; set; }
    public CanalPedido? CanalPedido { get; set; }
    public string? PerfilCliente { get; set; }
}
