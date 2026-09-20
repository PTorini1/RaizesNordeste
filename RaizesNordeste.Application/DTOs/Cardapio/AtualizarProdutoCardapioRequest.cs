namespace RaizesNordeste.Application.DTOs.Cardapio;

public class AtualizarProdutoCardapioRequest
{
    public decimal Preco { get; set; }
    public bool Disponivel { get; set; }
    public DateTime? InicioVigencia { get; set; }
    public DateTime? FimVigencia { get; set; }
}
