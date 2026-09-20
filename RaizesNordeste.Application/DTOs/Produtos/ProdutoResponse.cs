namespace RaizesNordeste.Application.DTOs.Produtos;

public class ProdutoResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal PrecoBase { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public bool Sazonal { get; set; }
    public bool Ativo { get; set; }
}
