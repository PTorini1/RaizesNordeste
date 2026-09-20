namespace RaizesNordeste.Domain.Entities;

public class Cliente
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string? Cpf { get; set; }
    public string? Telefone { get; set; }
    public DateTime? DataNascimento { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public Usuario? Usuario { get; set; }
    public ContaFidelidade? ContaFidelidade { get; set; }
    public List<ConsentimentoLGPD> ConsentimentoLgpd { get; set; } = [];
}
