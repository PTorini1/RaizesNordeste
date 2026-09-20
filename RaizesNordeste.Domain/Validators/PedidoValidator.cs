using FluentValidation;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Domain.Validators;

public class PedidoValidator : AbstractValidator<Pedido>
{
    public PedidoValidator()
    {
        RuleFor(p => p.ClienteId)
            .GreaterThan(0)
            .When(p => p.ClienteId.HasValue)
            .WithMessage("Cliente inválido.");

        RuleFor(p => p.UnidadeId)
            .GreaterThan(0)
            .WithMessage("Unidade inválida.");

        RuleFor(p => p.CanalPedido)
            .IsInEnum()
            .WithMessage("Canal de pedido inválido ou não preenchido.");

        RuleFor(p => p.Itens)
            .NotEmpty()
            .WithMessage("O pedido deve possuir pelo menos um item.");

        RuleForEach(p => p.Itens)
            .SetValidator(new ItemPedidoValidator());
    }
}

public class ItemPedidoValidator : AbstractValidator<ItemPedido>
{
    public ItemPedidoValidator()
    {
        RuleFor(i => i.ProdutoId)
            .GreaterThan(0)
            .WithMessage("Produto inválido no pedido.");

        RuleFor(i => i.PedidoId)
            .GreaterThan(0)
            .When(i => i.PedidoId != 0)
            .WithMessage("Pedido inválido no item.");

        RuleFor(i => i.Quantidade)
            .GreaterThan(0)
            .WithMessage(i => $"Quantidade inválida para o produto ID {i.ProdutoId}.");

        RuleFor(i => i.ValorUnitario)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Valor unitário do item não pode ser negativo.");

        RuleFor(i => i.ValorTotal)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Valor total do item não pode ser negativo.");
    }
}
