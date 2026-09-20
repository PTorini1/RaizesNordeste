namespace RaizesNordeste.Application.DTOs.Unidades;

public class UnidadeResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public string TipoOperacao { get; set; } = string.Empty;
    public bool Ativa { get; set; }
}
