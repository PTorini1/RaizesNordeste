namespace RaizesNordeste.Domain.Entities;

public class CardapioUnidade
{
    public int Id { get; set; }
    public int UnidadeId { get; set; }
    public int ProdutoId { get; set; }
    public decimal Preco { get; set; }
    public bool Disponivel { get; set; } = true;
    public DateTime? InicioVigencia { get; set; }
    public DateTime? FimVigencia { get; set; }

    public Unidade? Unidade { get; set; }
    public Produto? Produto { get; set; }
}
