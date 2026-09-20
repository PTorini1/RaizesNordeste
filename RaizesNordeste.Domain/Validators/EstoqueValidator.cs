using FluentValidation;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Validators;

public class EstoqueValidator : AbstractValidator<Estoque>
{
    public EstoqueValidator()
    {
        RuleFor(e => e.UnidadeId).GreaterThan(0).WithMessage("Unidade inválida.");
        RuleFor(e => e.ProdutoId).GreaterThan(0).WithMessage("Produto inválido.");
        RuleFor(e => e.QuantidadeAtual).GreaterThanOrEqualTo(0);
        RuleFor(e => e.QuantidadeMinima).GreaterThanOrEqualTo(0);
    }
}
