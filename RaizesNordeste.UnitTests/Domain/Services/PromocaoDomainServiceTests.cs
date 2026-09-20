using FluentAssertions;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Services;
using RaizesNordeste.UnitTests.Helpers;

namespace RaizesNordeste.UnitTests.Domain.Services;

public class PromocaoDomainServiceTests
{
    private readonly PromocaoDomainService _service = new();

    [Fact]
    public void CriarPromocao_DeveLancarDomainException_QuandoTipoDescontoForInvalido()
    {
        var act = () => _service.CriarPromocao(
            "Promo",
            null,
            "INVALIDO",
            10m,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1),
            null,
            null,
            null);

        act.Should().Throw<DomainException>()
            .WithMessage("*Tipo de desconto inválido*");
    }

    [Fact]
    public void CriarPromocao_DeveLancarDomainException_QuandoVigenciaForInvalida()
    {
        var act = () => _service.CriarPromocao(
            "Promo",
            null,
            "PERCENTUAL",
            10m,
            DateTime.UtcNow.AddDays(2),
            DateTime.UtcNow.AddDays(1),
            null,
            null,
            null);

        act.Should().Throw<DomainException>()
            .WithMessage("*data de início*menor*data de fim*");
    }

    [Fact]
    public void CalcularDesconto_DeveAplicarPromocaoPorProduto()
    {
        var pedido = Builders.Pedido();
        pedido.AdicionarItem(produtoId: 7, quantidade: 2, valorUnitario: 50m);
        var promocoes = new[] { Builders.Promocao(tipoDesconto: "PERCENTUAL", valorDesconto: 10m, produtoId: 7) };

        var (desconto, promocaoAplicada) = _service.CalcularDesconto(pedido, promocoes);

        desconto.Should().Be(10m);
        promocaoAplicada.Should().Be(promocoes[0]);
    }

    [Fact]
    public void CalcularDesconto_DeveAplicarPromocaoPorCanal_QuandoNaoHouverPromocaoPorProduto()
    {
        var pedido = Builders.Pedido(canal: CanalPedido.Totem);
        pedido.AdicionarItem(produtoId: 7, quantidade: 2, valorUnitario: 50m);
        var promocoes = new[] { Builders.Promocao(tipoDesconto: "VALOR", valorDesconto: 15m, canal: CanalPedido.Totem) };

        var (desconto, promocaoAplicada) = _service.CalcularDesconto(pedido, promocoes);

        desconto.Should().Be(15m);
        promocaoAplicada.Should().Be(promocoes[0]);
    }
}
