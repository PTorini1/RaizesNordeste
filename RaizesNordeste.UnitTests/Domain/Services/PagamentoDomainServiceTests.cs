using FluentAssertions;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Services;
using RaizesNordeste.UnitTests.Helpers;

namespace RaizesNordeste.UnitTests.Domain.Services;

public class PagamentoDomainServiceTests
{
    private readonly PagamentoDomainService _service = new();

    [Fact]
    public void CriarPagamento_DeveCriarPagamentoSolicitado_QuandoDadosForemValidos()
    {
        var pagamento = _service.CriarPagamento(1, "PIX", "chave-123");

        pagamento.Status.Should().Be(StatusPagamento.Solicitado);
        pagamento.PedidoId.Should().Be(1);
    }

    [Fact]
    public void CriarPagamento_DeveLancarDomainException_QuandoProvedorForVazio()
    {
        var act = () => _service.CriarPagamento(1, string.Empty, "chave-123");

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void ValidarIdempotencia_DeveNaoLancar_QuandoPagamentoExistenteTiverMesmaChave()
    {
        var pagamento = Builders.Pagamento(chaveIdempotencia: "abc");

        var act = () => _service.ValidarIdempotencia(pagamento, "abc");

        act.Should().NotThrow();
    }

    [Fact]
    public void ValidarIdempotencia_DeveLancarDomainException_QuandoChaveForDiferente()
    {
        var pagamento = Builders.Pagamento(chaveIdempotencia: "abc");

        var act = () => _service.ValidarIdempotencia(pagamento, "def");

        act.Should().Throw<DomainException>()
            .WithMessage("Transação de pagamento duplicada em andamento ou chave de idempotência expirada.");
    }

    [Fact]
    public void AtualizarAposProcessamento_DeveMarcarPagamentoComoAprovado_QuandoSucesso()
    {
        var pagamento = Builders.Pagamento();

        _service.AtualizarAposProcessamento(pagamento, true, "tx-1", "{}", "{}");

        pagamento.Status.Should().Be(StatusPagamento.Aprovado);
        pagamento.TransacaoExternaId.Should().Be("tx-1");
        pagamento.RespondidoEm.Should().NotBeNull();
    }
}
