namespace RaizesNordeste.Application.DTOs.Fidelidade;

public class ContaFidelidadeResponse
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public int SaldoPontos { get; set; }
    public bool Ativa { get; set; }
    public DateTime CriadoEm { get; set; }
    public List<MovimentacaoPontosResponse> Movimentacoes { get; set; } = [];
}

public class MovimentacaoPontosResponse
{
    public int Id { get; set; }
    public int? PedidoId { get; set; }
    public string TipoMovimentacao { get; set; } = string.Empty;
    public int Pontos { get; set; }
    public string? Descricao { get; set; }
    public DateTime CriadoEm { get; set; }
}
