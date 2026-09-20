using FluentAssertions;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Services;
using RaizesNordeste.UnitTests.Helpers;

namespace RaizesNordeste.UnitTests.Domain.Services;

public class ProdutoDomainServiceTests
{
    private readonly ProdutoDomainService _service = new();

    [Fact]
    public void CriarProduto_DeveCriarProduto_QuandoDadosForemValidos()
    {
        var produto = _service.CriarProduto("Cuscuz", "Cuscuz nordestino", 12.50m, "Comidas", false, true);

        produto.Nome.Should().Be("Cuscuz");
        produto.Ativo.Should().BeTrue();
    }

    [Fact]
    public void CriarProduto_DeveLancarDomainException_QuandoPrecoForZero()
    {
        var act = () => _service.CriarProduto("Cuscuz", null, 0, "Comidas", false, true);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void AtualizarProduto_DeveLancarDomainException_QuandoCategoriaUltrapassarMaxLength()
    {
        var produto = Builders.Produto();
        var categoriaGrande = new string('C', 101);

        var act = () => _service.AtualizarProduto(produto, "Cuscuz", null, 10m, categoriaGrande, false, true);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void ValidarAtivo_DeveLancarDomainException_QuandoProdutoEstiverInativo()
    {
        var produto = Builders.Produto(ativo: false);

        var act = () => _service.ValidarAtivo(produto);

        act.Should().Throw<DomainException>()
            .WithMessage("Produto inválido ou inativo.");
    }
}
