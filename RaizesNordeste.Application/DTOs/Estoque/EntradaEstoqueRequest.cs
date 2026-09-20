namespace RaizesNordeste.Application.DTOs.Estoque;

public class EntradaEstoqueRequest
{
    public int UnidadeId { get; set; }
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public int UsuarioId { get; set; }
    public string Motivo { get; set; } = string.Empty;
}
