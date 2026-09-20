using FluentValidation;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Validators;

public class MovimentacaoPontosValidator : AbstractValidator<MovimentacaoPontos>
{
    public MovimentacaoPontosValidator()
    {
        RuleFor(m => m.ContaFidelidadeId).GreaterThan(0).WithMessage("Conta fidelidade inválida.");
        RuleFor(m => m.PedidoId).GreaterThan(0).When(m => m.PedidoId.HasValue).WithMessage("Pedido inválido.");
        RuleFor(m => m.TipoMovimentacao).IsInEnum();
        RuleFor(m => m.Pontos).GreaterThan(0);
        RuleFor(m => m.Descricao).MaximumLength(255);
    }
}
