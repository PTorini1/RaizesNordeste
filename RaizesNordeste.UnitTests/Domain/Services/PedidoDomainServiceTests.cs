using FluentAssertions;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Services;
using RaizesNordeste.UnitTests.Helpers;

namespace RaizesNordeste.UnitTests.Domain.Services;

public class PedidoDomainServiceTests
{
    private readonly PedidoDomainService _service = new();

    [Fact]
    public void ValidarUnidadeAtiva_DeveLancarDomainException_QuandoUnidadeEstiverInativa()
    {
        var unidade = Builders.Unidade(ativa: false);

        var act = () => _service.ValidarUnidadeAtiva(unidade);

        act.Should().Throw<DomainException>()
            .WithMessage("Unidade inválida ou inativa.");
    }

    [Fact]
    public void ValidarItensPresentes_DeveLancarDomainException_QuandoListaEstiverVazia()
    {
        var act = () => _service.ValidarItensPresentes(Array.Empty<int>());

        act.Should().Throw<DomainException>()
            .WithMessage("O pedido deve possuir pelo menos um item.");
    }

    [Fact]
    public void ValidarDisponibilidadeNoCardapio_DeveLancarDomainException_QuandoItemEstiverIndisponivel()
    {
        var item = Builders.CardapioItem(produtoId: 10, disponivel: false);

        var act = () => _service.ValidarDisponibilidadeNoCardapio(item, 10);

        act.Should().Throw<DomainException>()
            .WithMessage("O produto ID 10 não está disponível no cardápio desta unidade.");
    }

    [Fact]
    public void CriarPedido_DeveCriarPedidoComStatusCriado()
    {
        var pedido = _service.CriarPedido(clienteId: 1, unidadeId: 2, CanalPedido.Web);

        pedido.ClienteId.Should().Be(1);
        pedido.UnidadeId.Should().Be(2);
        pedido.CanalPedido.Should().Be(CanalPedido.Web);
        pedido.Status.Should().Be(StatusPedido.Criado);
    }

    [Fact]
    public void CriarMovimentacaoVenda_DeveLancarDomainException_QuandoUsuarioForInvalido()
    {
        var act = () => _service.CriarMovimentacaoVenda(estoqueId: 1, usuarioId: 0, quantidade: 1, pedidoId: 10);

        act.Should().Throw<DomainException>()
            .WithMessage("*Usuário inválido*");
    }

    [Fact]
    public void CriarMovimentacaoEstornoAjuste_DeveCriarMovimentacaoDeAjuste()
    {
        var movimentacao = _service.CriarMovimentacaoEstornoAjuste(estoqueId: 1, usuarioId: 2, quantidade: 3, pedidoId: 10, motivo: "Cliente");

        movimentacao.TipoMovimentacao.Should().Be(TipoMovimentacaoEstoque.Ajuste);
        movimentacao.Motivo.Should().Contain("Pedido ID 10");
    }
}
