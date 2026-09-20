using FluentValidation;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Validators;

public class ConsentimentoLGPDValidator : AbstractValidator<ConsentimentoLGPD>
{
    public ConsentimentoLGPDValidator()
    {
        RuleFor(c => c.ClienteId).GreaterThan(0).WithMessage("Cliente inválido.");
        RuleFor(c => c.Finalidade).NotEmpty().MaximumLength(150);
        RuleFor(c => c.VersaoTermo).NotEmpty().MaximumLength(50);
    }
}
