using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Exceptions;

namespace RaizesNordeste.Domain.Services;

public class FidelidadeDomainService
{
    public void ValidarClientePodeAderir(Cliente? cliente)
    {
        if (cliente is null || !cliente.Ativo)
        {
            throw new DomainException("Cliente inválido ou inativo.");
        }
    }

    public ContaFidelidade CriarOuAtualizarConta(int clienteId, ContaFidelidade? conta, bool consentido)
    {
        if (conta is null)
        {
            return new ContaFidelidade
            {
                ClienteId = clienteId,
                SaldoPontos = 0,
                Ativa = consentido,
                CriadoEm = DateTime.UtcNow
            };
        }

        conta.Ativa = consentido;
        return conta;
    }

    public int CalcularPontos(Pedido pedido)
    {
        return (int)(pedido.ValorTotal / 10m);
    }

    public MovimentacaoPontos? AcumularPontos(ContaFidelidade conta, Pedido pedido)
    {
        if (!conta.Ativa)
        {
            throw new DomainException("A conta fidelidade do cliente está inativa.");
        }

        var pontos = CalcularPontos(pedido);
        if (pontos <= 0)
        {
            return null;
        }

        conta.SaldoPontos += pontos;

        return new MovimentacaoPontos
        {
            ContaFidelidadeId = conta.Id,
            PedidoId = pedido.Id,
            TipoMovimentacao = TipoMovimentacaoPontos.Acumulo,
            Pontos = pontos,
            Descricao = $"Acumulo ref. Pedido ID {pedido.Id}",
            CriadoEm = DateTime.UtcNow
        };
    }

    public MovimentacaoPontos ResgatarPontos(ContaFidelidade conta, int pontos, string? descricao)
    {
        if (pontos <= 0)
        {
            throw new DomainException("A quantidade de pontos para resgate deve ser maior que zero.");
        }

        if (!conta.Ativa)
        {
            throw new DomainException("O cliente não possui uma conta fidelidade ativa.");
        }

        if (conta.SaldoPontos < pontos)
        {
            throw new DomainException($"Saldo de pontos insuficiente. Disponível: {conta.SaldoPontos}, Solicitado: {pontos}.");
        }

        conta.SaldoPontos -= pontos;

        return new MovimentacaoPontos
        {
            ContaFidelidadeId = conta.Id,
            PedidoId = null,
            TipoMovimentacao = TipoMovimentacaoPontos.Resgate,
            Pontos = pontos,
            Descricao = string.IsNullOrWhiteSpace(descricao) ? "Resgate de pontos" : descricao,
            CriadoEm = DateTime.UtcNow
        };
    }
}
