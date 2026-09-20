using FluentAssertions;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Services;
using RaizesNordeste.UnitTests.Helpers;

namespace RaizesNordeste.UnitTests.Domain.Services;

public class EstoqueDomainServiceTests
{
    private readonly EstoqueDomainService _service = new();

    [Fact]
    public void BaixarEstoque_DeveReduzirQuantidade_QuandoHouverSaldo()
    {
        var estoque = Builders.Estoque(quantidadeAtual: 10);

        _service.BaixarEstoque(estoque, 4);

        estoque.QuantidadeAtual.Should().Be(6);
    }

    [Fact]
    public void BaixarEstoque_DeveLancarDomainException_QuandoQuantidadeForMaiorQueSaldo()
    {
        var estoque = Builders.Estoque(quantidadeAtual: 3);

        var act = () => _service.BaixarEstoque(estoque, 4);

        act.Should().Throw<DomainException>()
            .WithMessage("*Estoque insuficiente*");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void RegistrarMovimentacao_DeveLancarDomainException_QuandoQuantidadeForMenorOuIgualAZero(int quantidade)
    {
        var estoque = Builders.Estoque();

        var act = () => _service.RegistrarMovimentacao(estoque, TipoMovimentacaoEstoque.Entrada, quantidade);

        act.Should().Throw<DomainException>()
            .WithMessage("A quantidade da movimentação deve ser maior que zero.");
    }

    [Fact]
    public void RegistrarMovimentacao_DeveSomarQuantidade_QuandoTipoForEntrada()
    {
        var estoque = Builders.Estoque(quantidadeAtual: 10);

        var anterior = _service.RegistrarMovimentacao(estoque, TipoMovimentacaoEstoque.Entrada, 5);

        anterior.Should().Be(10);
        estoque.QuantidadeAtual.Should().Be(15);
    }

    [Fact]
    public void ValidarSaida_DeveLancarDomainException_QuandoTipoForEntrada()
    {
        var act = () => _service.ValidarSaida(TipoMovimentacaoEstoque.Entrada);

        act.Should().Throw<DomainException>()
            .WithMessage("Tipo de movimentação inválido para saída de estoque.");
    }
}
