using FluentValidation;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Validators;

public class ClienteValidator : AbstractValidator<Cliente>
{
    public ClienteValidator()
    {
        RuleFor(c => c.UsuarioId).GreaterThan(0).WithMessage("Usuário inválido.");
        RuleFor(c => c.Cpf).MaximumLength(14);
        RuleFor(c => c.Telefone).MaximumLength(20);
    }
}
