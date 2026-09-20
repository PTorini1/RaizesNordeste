namespace RaizesNordesteApi.Contracts.Responses;

public class RespostaErroApi
{
    public int Status { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public string RastreamentoId { get; set; } = string.Empty;
    public string Caminho { get; set; } = string.Empty;
    public DateTimeOffset DataHora { get; set; } = DateTimeOffset.UtcNow;
    public IReadOnlyDictionary<string, string[]>? Erros { get; set; }
}
