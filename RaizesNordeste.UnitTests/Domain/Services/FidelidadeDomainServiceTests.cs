using FluentAssertions;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Services;
using RaizesNordeste.UnitTests.Helpers;

namespace RaizesNordeste.UnitTests.Domain.Services;

public class FidelidadeDomainServiceTests
{
    private readonly FidelidadeDomainService _service = new();

    [Fact]
    public void ValidarClientePodeAderir_DeveLancarDomainException_QuandoClienteEstiverInativo()
    {
        var cliente = Builders.Cliente(ativo: false);

        var act = () => _service.ValidarClientePodeAderir(cliente);

        act.Should().Throw<DomainException>()
            .WithMessage("Cliente inválido ou inativo.");
    }

    [Fact]
    public void AcumularPontos_DeveSomarUmPontoACadaDezReais()
    {
        var conta = Builders.ContaFidelidade(saldo: 5);
        var pedido = Builders.Pedido(valorTotal: 129.90m);

        var movimentacao = _service.AcumularPontos(conta, pedido);

        conta.SaldoPontos.Should().Be(17);
        movimentacao.Should().NotBeNull();
        movimentacao!.Pontos.Should().Be(12);
    }

    [Fact]
    public void AcumularPontos_DeveRetornarNull_QuandoPedidoNaoGerarPontos()
    {
        var conta = Builders.ContaFidelidade(saldo: 5);
        var pedido = Builders.Pedido(valorTotal: 9.99m);

        var movimentacao = _service.AcumularPontos(conta, pedido);

        movimentacao.Should().BeNull();
        conta.SaldoPontos.Should().Be(5);
    }

    [Fact]
    public void ResgatarPontos_DeveDebitarSaldo_QuandoSaldoForSuficiente()
    {
        var conta = Builders.ContaFidelidade(saldo: 100);

        var movimentacao = _service.ResgatarPontos(conta, 40, "Desconto");

        conta.SaldoPontos.Should().Be(60);
        movimentacao.Pontos.Should().Be(40);
    }

    [Fact]
    public void ResgatarPontos_DeveLancarDomainException_QuandoSaldoForInsuficiente()
    {
        var conta = Builders.ContaFidelidade(saldo: 10);

        var act = () => _service.ResgatarPontos(conta, 20, "Desconto");

        act.Should().Throw<DomainException>()
            .WithMessage("*Saldo de pontos insuficiente*");
    }
}
