using FluentValidation;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Validators;

public class CardapioUnidadeValidator : AbstractValidator<CardapioUnidade>
{
    public CardapioUnidadeValidator()
    {
        RuleFor(c => c.UnidadeId).GreaterThan(0).WithMessage("Unidade inválida.");
        RuleFor(c => c.ProdutoId).GreaterThan(0).WithMessage("Produto inválido.");
        RuleFor(c => c.Preco).GreaterThanOrEqualTo(0).WithMessage("Preço do cardápio não pode ser negativo.");
        RuleFor(c => c)
            .Must(c => !c.InicioVigencia.HasValue || !c.FimVigencia.HasValue || c.InicioVigencia < c.FimVigencia)
            .WithMessage("A vigência inicial do produto no cardápio deve ser menor que a vigência final.");
    }
}
