using RaizesNordeste.Domain.Enums;

namespace RaizesNordeste.Domain.Entities;

public class Promocao
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string TipoDesconto { get; set; } = string.Empty; 
    public decimal ValorDesconto { get; set; }
    public DateTime InicioVigencia { get; set; }
    public DateTime FimVigencia { get; set; }
    public bool Ativa { get; set; } = true;

    public int? ProdutoId { get; set; }
    public CanalPedido? CanalPedido { get; set; }
    public string? PerfilCliente { get; set; }

    public Produto? Produto { get; set; }
}
