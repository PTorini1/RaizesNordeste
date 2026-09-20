namespace RaizesNordeste.Domain.Entities;

public class ContaFidelidade
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int SaldoPontos { get; set; } = 0;
    public bool Ativa { get; set; } = true;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public Cliente? Cliente { get; set; }
}
