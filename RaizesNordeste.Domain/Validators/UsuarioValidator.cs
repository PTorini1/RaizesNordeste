using FluentValidation;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Validators;

public class UsuarioValidator : AbstractValidator<Usuario>
{
    public UsuarioValidator()
    {
        RuleFor(u => u.Nome).NotEmpty().MaximumLength(150);
        RuleFor(u => u.Email).NotEmpty().EmailAddress().MaximumLength(150);
        RuleFor(u => u.FirebaseUid).NotEmpty().MaximumLength(255);
        RuleFor(u => u.Role).IsInEnum();
    }
}
