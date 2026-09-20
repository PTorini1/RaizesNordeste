using FluentAssertions;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Services;
using RaizesNordeste.UnitTests.Helpers;

namespace RaizesNordeste.UnitTests.Domain.Services;

public class UnidadeDomainServiceTests
{
    private readonly UnidadeDomainService _service = new();

    [Fact]
    public void CriarUnidade_DeveCriarUnidadeAtiva_QuandoDadosForemValidos()
    {
        var unidade = _service.CriarUnidade("Loja Centro", "Fortaleza", "CE", "Rua A, 100", "Loja");

        unidade.Ativa.Should().BeTrue();
        unidade.Estado.Should().Be("CE");
    }

    [Fact]
    public void CriarUnidade_DeveLancarDomainException_QuandoEstadoTiverMaisDeDoisCaracteres()
    {
        var act = () => _service.CriarUnidade("Loja Centro", "Fortaleza", "STRING", "Rua A, 100", "Loja");

        act.Should().Throw<DomainException>()
            .WithMessage("*estado*2 caracteres*");
    }

    [Fact]
    public void AtualizarUnidade_DeveLancarDomainException_QuandoNomeUltrapassarMaxLength()
    {
        var unidade = Builders.Unidade();
        var nomeGrande = new string('A', 151);

        var act = () => _service.AtualizarUnidade(unidade, nomeGrande, "Fortaleza", "CE", "Rua A, 100", "Loja", true);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void ValidarAtiva_DeveLancarDomainException_QuandoUnidadeEstiverInativa()
    {
        var unidade = Builders.Unidade(ativa: false);

        var act = () => _service.ValidarAtiva(unidade);

        act.Should().Throw<DomainException>()
            .WithMessage("Unidade inválida ou inativa.");
    }
}
