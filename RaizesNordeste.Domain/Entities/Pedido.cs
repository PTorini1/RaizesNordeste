using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Validators;

namespace RaizesNordeste.Domain.Entities;

public class Pedido
{
    public int Id { get; set; }
    public int? ClienteId { get; set; }
    public int UnidadeId { get; set; }
    public CanalPedido CanalPedido { get; set; }
    public StatusPedido Status { get; set; } = StatusPedido.Criado;
    public decimal ValorTotal { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public DateTime? AtualizadoEm { get; set; }

    public Cliente? Cliente { get; set; }
    public Unidade? Unidade { get; set; }
    public List<ItemPedido> Itens { get; set; } = [];
    public Pagamento? Pagamento { get; set; }

    public string? MotivoCancelamento { get; set; }
    public int? UsuarioCancelamentoId { get; set; }
    public DateTime? CanceladoEm { get; set; }
    public Usuario? UsuarioCancelamento { get; set; }

    public void Validar()
    {
        new PedidoValidator().ValidaOuLancaExcecao(this);
    }

    public void AdicionarItem(int produtoId, int quantidade, decimal valorUnitario)
    {
        var item = new ItemPedido
        {
            ProdutoId = produtoId,
            Quantidade = quantidade,
            ValorUnitario = valorUnitario,
            ValorTotal = quantidade * valorUnitario
        };

        new ItemPedidoValidator().ValidaOuLancaExcecao(item);
        Itens.Add(item);
    }

    public void AplicarDesconto(decimal desconto)
    {
        var valorBase = Itens.Sum(i => i.ValorTotal);

        if (desconto < 0)
        {
            throw new DomainException("O desconto do pedido não pode ser negativo.");
        }

        ValorTotal = valorBase - Math.Min(desconto, valorBase);
    }

    public void Cancelar(int usuarioId, string motivo)
    {
        if (Status == StatusPedido.Cancelado)
        {
            return;
        }

        if (Status == StatusPedido.Entregue)
        {
            throw new DomainException("Não é possível cancelar um pedido que já foi entregue.");
        }

        MotivoCancelamento = motivo;
        UsuarioCancelamentoId = usuarioId;
        CanceladoEm = DateTime.UtcNow;

        AlterarStatus(StatusPedido.Cancelado);
    }

    public void AlterarStatus(StatusPedido novoStatus)
    {
        if (Status == novoStatus)
        {
            return;
        }

        if (Status == StatusPedido.Cancelado)
        {
            throw new DomainException("Um pedido cancelado não pode ter seu status alterado.");
        }

        if (Status == StatusPedido.Entregue)
        {
            throw new DomainException("Um pedido entregue é um estado final e não pode ser alterado.");
        }

        if (!PodeAlterarPara(novoStatus))
        {
            throw new DomainException($"Transição de status inválida: {Status} -> {novoStatus}.");
        }

        Status = novoStatus;
        AtualizadoEm = DateTime.UtcNow;
    }

    private bool PodeAlterarPara(StatusPedido novoStatus)
    {
        return Status switch
        {
            StatusPedido.Criado => novoStatus is StatusPedido.AguardandoPagamento or StatusPedido.EmPreparo or StatusPedido.Cancelado,
            StatusPedido.AguardandoPagamento => novoStatus is StatusPedido.PagamentoAprovado or StatusPedido.PagamentoRecusado or StatusPedido.Cancelado,
            StatusPedido.PagamentoAprovado => novoStatus is StatusPedido.EmPreparo or StatusPedido.Cancelado,
            StatusPedido.PagamentoRecusado => novoStatus is StatusPedido.AguardandoPagamento or StatusPedido.Cancelado,
            StatusPedido.EmPreparo => novoStatus is StatusPedido.Pronto or StatusPedido.Cancelado,
            StatusPedido.Pronto => novoStatus is StatusPedido.Entregue or StatusPedido.Cancelado,
            _ => false
        };
    }
}
