namespace RaizesNordeste.Domain.Entities;

public class ConsentimentoLGPD
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string Finalidade { get; set; } = string.Empty;
    public bool Consentido { get; set; }
    public string VersaoTermo { get; set; } = string.Empty;
    public DateTime? ConcedidoEm { get; set; }
    public DateTime? RevogadoEm { get; set; }

    public Cliente? Cliente { get; set; }
}
