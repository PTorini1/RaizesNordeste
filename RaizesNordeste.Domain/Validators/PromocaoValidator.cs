using FluentValidation;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Validators;

public class PromocaoValidator : AbstractValidator<Promocao>
{
    public PromocaoValidator()
    {
        RuleFor(p => p.Nome)
            .NotEmpty()
            .MaximumLength(150)
            .WithMessage("O nome da promoção deve ser informado.");

        RuleFor(p => p.Descricao)
            .MaximumLength(500);

        RuleFor(p => p.TipoDesconto)
            .NotEmpty()
            .MaximumLength(50)
            .Must(tipo => string.Equals(tipo, "PERCENTUAL", StringComparison.OrdinalIgnoreCase) ||
                          string.Equals(tipo, "VALOR", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Tipo de desconto inválido. Valores permitidos: PERCENTUAL ou VALOR.");

        RuleFor(p => p.ValorDesconto)
            .GreaterThan(0)
            .WithMessage("O valor do desconto deve ser maior que zero.");

        RuleFor(p => p)
            .Must(p => p.InicioVigencia < p.FimVigencia)
            .WithMessage("A data de início da vigência deve ser menor que a data de fim.");

        RuleFor(p => p.ProdutoId)
            .GreaterThan(0)
            .When(p => p.ProdutoId.HasValue)
            .WithMessage("Produto inválido.");

        RuleFor(p => p.CanalPedido)
            .IsInEnum()
            .When(p => p.CanalPedido.HasValue);
    }
}
