namespace RaizesNordeste.Application.DTOs.Produtos;

public class CriarProdutoRequest
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal PrecoBase { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public bool Sazonal { get; set; }
    public bool Ativo { get; set; } = true;
}

public class AtualizarProdutoRequest
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal PrecoBase { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public bool Sazonal { get; set; }
    public bool Ativo { get; set; } = true;
}

public class AtualizarParcialProdutoRequest
{
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public decimal? PrecoBase { get; set; }
    public string? Categoria { get; set; }
    public bool? Sazonal { get; set; }
    public bool? Ativo { get; set; }
}
