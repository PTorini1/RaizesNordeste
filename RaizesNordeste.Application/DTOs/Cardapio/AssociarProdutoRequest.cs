namespace RaizesNordeste.Application.DTOs.Cardapio;

public class AssociarProdutoRequest
{
    public int ProdutoId { get; set; }
    public decimal Preco { get; set; }
    public bool Disponivel { get; set; } = true;
    public DateTime? InicioVigencia { get; set; }
    public DateTime? FimVigencia { get; set; }
}
