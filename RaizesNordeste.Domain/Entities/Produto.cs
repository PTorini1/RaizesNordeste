namespace RaizesNordeste.Domain.Entities;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal PrecoBase { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public bool Sazonal { get; set; } = false;
    public bool Ativo { get; set; } = true;
}
