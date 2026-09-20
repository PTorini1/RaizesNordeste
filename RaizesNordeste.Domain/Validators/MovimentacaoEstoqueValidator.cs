using FluentValidation;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Validators;

public class MovimentacaoEstoqueValidator : AbstractValidator<MovimentacaoEstoque>
{
    public MovimentacaoEstoqueValidator()
    {
        RuleFor(m => m.EstoqueId).GreaterThan(0).WithMessage("Estoque inválido.");
        RuleFor(m => m.UsuarioId).GreaterThan(0).WithMessage("Usuário inválido.");
        RuleFor(m => m.TipoMovimentacao).IsInEnum();
        RuleFor(m => m.Quantidade).GreaterThan(0);
        RuleFor(m => m.Motivo).MaximumLength(255);
    }
}
