namespace RaizesNordeste.Domain.Entities;

public class Auditoria
{
    public int Id { get; set; }
    public int? UsuarioId { get; set; }
    public string Entidade { get; set; } = string.Empty;
    public int EntidadeId { get; set; }
    public string Acao { get; set; } = string.Empty;
    public string? DadosAnteriores { get; set; }
    public string? DadosNovos { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public Usuario? Usuario { get; set; }
}
