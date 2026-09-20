using FluentValidation;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Validators;

public class AuditoriaValidator : AbstractValidator<Auditoria>
{
    public AuditoriaValidator()
    {
        RuleFor(a => a.UsuarioId).GreaterThan(0).When(a => a.UsuarioId.HasValue).WithMessage("Usuário inválido.");
        RuleFor(a => a.Entidade).NotEmpty().MaximumLength(100);
        RuleFor(a => a.EntidadeId).GreaterThan(0);
        RuleFor(a => a.Acao).NotEmpty().MaximumLength(100);
    }
}
