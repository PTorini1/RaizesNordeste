namespace RaizesNordeste.Application.DTOs.Fidelidade;

public class AderirFidelidadeRequest
{
    public int ClienteId { get; set; }
    public string Finalidade { get; set; } = "FIDELIDADE";
    public bool Consentido { get; set; } = true;
    public string VersaoTermo { get; set; } = "v1.0";
}
