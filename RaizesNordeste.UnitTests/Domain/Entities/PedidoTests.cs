using FluentAssertions;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.UnitTests.Helpers;

namespace RaizesNordeste.UnitTests.Domain.Entities;

public class PedidoTests
{
    [Fact]
    public void AdicionarItem_DeveAdicionarItemECalcularValorTotal()
    {
        var pedido = Builders.Pedido();

        pedido.AdicionarItem(produtoId: 1, quantidade: 3, valorUnitario: 12m);

        pedido.Itens.Should().ContainSingle();
        pedido.Itens[0].ValorTotal.Should().Be(36m);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AdicionarItem_DeveLancarDomainException_QuandoQuantidadeForMenorOuIgualAZero(int quantidade)
    {
        var pedido = Builders.Pedido();

        var act = () => pedido.AdicionarItem(produtoId: 1, quantidade, valorUnitario: 12m);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void AlterarStatus_DevePermitirFluxoOperacionalValido()
    {
        var pedido = Builders.Pedido();

        pedido.AlterarStatus(StatusPedido.EmPreparo);
        pedido.AlterarStatus(StatusPedido.Pronto);
        pedido.AlterarStatus(StatusPedido.Entregue);

        pedido.Status.Should().Be(StatusPedido.Entregue);
    }

    [Fact]
    public void AlterarStatus_DeveLancarDomainException_QuandoTransicaoForInvalida()
    {
        var pedido = Builders.Pedido();

        var act = () => pedido.AlterarStatus(StatusPedido.Entregue);

        act.Should().Throw<DomainException>()
            .WithMessage("*Transição de status inválida*");
    }

    [Fact]
    public void Cancelar_DeveLancarDomainException_QuandoPedidoJaFoiEntregue()
    {
        var pedido = Builders.Pedido(status: StatusPedido.Entregue);

        var act = () => pedido.Cancelar(usuarioId: 1, motivo: "Teste");

        act.Should().Throw<DomainException>()
            .WithMessage("Não é possível cancelar um pedido que já foi entregue.");
    }

    [Fact]
    public void AplicarDesconto_DeveLancarDomainException_QuandoDescontoForNegativo()
    {
        var pedido = Builders.PedidoComItem(valorUnitario: 50m);

        var act = () => pedido.AplicarDesconto(-1m);

        act.Should().Throw<DomainException>()
            .WithMessage("O desconto do pedido não pode ser negativo.");
    }
}
