namespace RaizesNordeste.Application.DTOs.Promocoes;

public class PromocaoResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string TipoDesconto { get; set; } = string.Empty;
    public decimal ValorDesconto { get; set; }
    public DateTime InicioVigencia { get; set; }
    public DateTime FimVigencia { get; set; }
    public bool Ativa { get; set; }
    public int? ProdutoId { get; set; }
    public string? ProdutoNome { get; set; }
    public string? CanalPedido { get; set; }
    public string? PerfilCliente { get; set; }
}
