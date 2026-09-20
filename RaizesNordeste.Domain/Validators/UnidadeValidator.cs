using FluentValidation;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Validators;

public class UnidadeValidator : AbstractValidator<Unidade>
{
    public UnidadeValidator()
    {
        RuleFor(u => u.Nome).NotEmpty().MaximumLength(150);
        RuleFor(u => u.Cidade).NotEmpty().MaximumLength(100);
        RuleFor(u => u.Estado)
            .NotEmpty()
            .Length(2)
            .WithMessage("O estado deve ser informado com a sigla de 2 caracteres.");
        RuleFor(u => u.Endereco).NotEmpty().MaximumLength(255);
        RuleFor(u => u.TipoOperacao).NotEmpty().MaximumLength(50);
    }
}
