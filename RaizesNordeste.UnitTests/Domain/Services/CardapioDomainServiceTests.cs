using FluentAssertions;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Services;
using RaizesNordeste.UnitTests.Helpers;

namespace RaizesNordeste.UnitTests.Domain.Services;

public class CardapioDomainServiceTests
{
    private readonly CardapioDomainService _service = new();

    [Fact]
    public void ValidarAssociacao_DeveLancarDomainException_QuandoUnidadeEstiverInativa()
    {
        var unidade = Builders.Unidade(ativa: false);
        var produto = Builders.Produto();

        var act = () => _service.ValidarAssociacao(unidade, produto, [], produto.Id);

        act.Should().Throw<DomainException>()
            .WithMessage("Unidade inválida ou inativa.");
    }

    [Fact]
    public void ValidarAssociacao_DeveLancarDomainException_QuandoProdutoEstiverDuplicadoNoCardapio()
    {
        var unidade = Builders.Unidade();
        var produto = Builders.Produto();
        var cardapio = new[] { Builders.CardapioItem(produtoId: produto.Id) };

        var act = () => _service.ValidarAssociacao(unidade, produto, cardapio, produto.Id);

        act.Should().Throw<DomainException>()
            .WithMessage("Este produto já está associado ao cardápio desta unidade.");
    }

    [Fact]
    public void CriarAssociacao_DeveLancarDomainException_QuandoPrecoForNegativo()
    {
        var act = () => _service.CriarAssociacao(1, 1, -1m, true, null, null);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void CriarAssociacao_DeveLancarDomainException_QuandoInicioForMaiorQueFim()
    {
        var inicio = DateTime.UtcNow.AddDays(2);
        var fim = DateTime.UtcNow.AddDays(1);

        var act = () => _service.CriarAssociacao(1, 1, 10m, true, inicio, fim);

        act.Should().Throw<DomainException>()
            .WithMessage("*vigência inicial*menor*vigência final*");
    }
}
