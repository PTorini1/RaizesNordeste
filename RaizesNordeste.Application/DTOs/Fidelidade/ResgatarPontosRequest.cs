namespace RaizesNordeste.Application.DTOs.Fidelidade;

public class ResgatarPontosRequest
{
    public int ClienteId { get; set; }
    public int Pontos { get; set; }
    public string Descricao { get; set; } = string.Empty;
}
