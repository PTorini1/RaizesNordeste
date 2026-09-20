namespace RaizesNordeste.Domain.Entities;

public class Estoque
{
    public int Id { get; set; }
    public int UnidadeId { get; set; }
    public int ProdutoId { get; set; }
    public int QuantidadeAtual { get; set; } = 0;
    public int QuantidadeMinima { get; set; } = 0;
    public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow;

    public Unidade? Unidade { get; set; }
    public Produto? Produto { get; set; }
}
