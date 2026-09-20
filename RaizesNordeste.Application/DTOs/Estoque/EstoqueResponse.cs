namespace RaizesNordeste.Application.DTOs.Estoque;

public class EstoqueResponse
{
    public int Id { get; set; }
    public int UnidadeId { get; set; }
    public int ProdutoId { get; set; }
    public string ProdutoNome { get; set; } = string.Empty;
    public int QuantidadeAtual { get; set; }
    public int QuantidadeMinima { get; set; }
    public DateTime AtualizadoEm { get; set; }
}
