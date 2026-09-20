using RaizesNordeste.Domain.Enums;

namespace RaizesNordeste.Domain.Entities;

public class MovimentacaoEstoque
{
    public int Id { get; set; }
    public int EstoqueId { get; set; }
    public int UsuarioId { get; set; }
    public TipoMovimentacaoEstoque TipoMovimentacao { get; set; }
    public int Quantidade { get; set; }
    public string? Motivo { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public Estoque? Estoque { get; set; }
    public Usuario? Usuario { get; set; }
}
