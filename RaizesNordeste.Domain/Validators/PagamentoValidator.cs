using FluentValidation;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Validators;

public class PagamentoValidator : AbstractValidator<Pagamento>
{
    public PagamentoValidator()
    {
        RuleFor(p => p.PedidoId).GreaterThan(0).WithMessage("Pedido inválido.");
        RuleFor(p => p.Provedor).NotEmpty().MaximumLength(100);
        RuleFor(p => p.Status).IsInEnum();
        RuleFor(p => p.TransacaoExternaId).MaximumLength(150);
        RuleFor(p => p.ChaveIdempotencia).NotEmpty().MaximumLength(150);
    }
}
