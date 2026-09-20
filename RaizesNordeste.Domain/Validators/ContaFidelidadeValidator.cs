using FluentValidation;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Validators;

public class ContaFidelidadeValidator : AbstractValidator<ContaFidelidade>
{
    public ContaFidelidadeValidator()
    {
        RuleFor(c => c.ClienteId).GreaterThan(0).WithMessage("Cliente inválido.");
        RuleFor(c => c.SaldoPontos).GreaterThanOrEqualTo(0);
    }
}
