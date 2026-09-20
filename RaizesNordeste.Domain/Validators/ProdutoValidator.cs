using FluentValidation;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Validators;

public class ProdutoValidator : AbstractValidator<Produto>
{
    public ProdutoValidator()
    {
        RuleFor(p => p.Nome)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(p => p.Descricao)
            .MaximumLength(500);

        RuleFor(p => p.PrecoBase)
            .GreaterThan(0);

        RuleFor(p => p.Categoria)
            .NotEmpty()
            .MaximumLength(100);
    }
}
