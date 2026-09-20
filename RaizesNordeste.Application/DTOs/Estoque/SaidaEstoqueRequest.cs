using RaizesNordeste.Domain.Enums;

namespace RaizesNordeste.Application.DTOs.Estoque;

public class SaidaEstoqueRequest
{
    public int UnidadeId { get; set; }
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public int UsuarioId { get; set; }
    public TipoMovimentacaoEstoque TipoMovimentacao { get; set; }
    public string Motivo { get; set; } = string.Empty;
}
