namespace RaizesNordeste.Application.DTOs.Cardapio;

public class CardapioResponse
{
    public int ProdutoId { get; set; }
    public string ProdutoNome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public bool Sazonal { get; set; }
    public bool Disponivel { get; set; }
}
