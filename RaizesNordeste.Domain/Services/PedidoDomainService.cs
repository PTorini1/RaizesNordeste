using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Validators;

namespace RaizesNordeste.Domain.Services;

public class PedidoDomainService
{
    public void ValidarUnidadeAtiva(Unidade? unidade)
    {
        if (unidade is null || !unidade.Ativa)
        {
            throw new DomainException("Unidade inválida ou inativa.");
        }
    }

    public void ValidarClienteAtivo(Cliente? cliente)
    {
        if (cliente is null || !cliente.Ativo)
        {
            throw new DomainException("Cliente inválido ou inativo.");
        }
    }

    public void ValidarItensPresentes<T>(ICollection<T>? itens)
    {
        if (itens is null || itens.Count == 0)
        {
            throw new DomainException("O pedido deve possuir pelo menos um item.");
        }
    }

    public void ValidarUsuarioAtivo(Usuario? usuario)
    {
        if (usuario is null || !usuario.Ativo)
        {
            throw new DomainException("Não há usuário ativo para registrar a movimentação de estoque.");
        }
    }

    public void ValidarDisponibilidadeNoCardapio(CardapioUnidade? cardapioItem, int produtoId)
    {
        if (cardapioItem is null || !cardapioItem.Disponivel)
        {
            throw new DomainException($"O produto ID {produtoId} não está disponível no cardápio desta unidade.");
        }
    }

    public void ValidarEstoqueExistente(Estoque? estoque, int produtoId) =>
       _ = estoque ?? throw new DomainException($"Estoque não encontrado para o produto ID {produtoId}.");

    public Pedido CriarPedido(int? clienteId, int unidadeId, CanalPedido canalPedido)
    {
        return new Pedido
        {
            ClienteId = clienteId,
            UnidadeId = unidadeId,
            CanalPedido = canalPedido,
            Status = StatusPedido.Criado,
            CriadoEm = DateTime.UtcNow
        };
    }

    public MovimentacaoEstoque CriarMovimentacaoVenda(int estoqueId, int usuarioId, int quantidade, int pedidoId)
    {
        var mov = new MovimentacaoEstoque
        {
            EstoqueId = estoqueId,
            UsuarioId = usuarioId,
            TipoMovimentacao = TipoMovimentacaoEstoque.Venda,
            Quantidade = quantidade,
            Motivo = "Venda - Pedido em criacao",
            CriadoEm = DateTime.UtcNow
        };

        new MovimentacaoEstoqueValidator().ValidaOuLancaExcecao(mov);
        return mov;
    }

    public MovimentacaoEstoque CriarMovimentacaoEstornoAjuste(int estoqueId, int usuarioId, int quantidade, int pedidoId, string motivo)
    {
        var mov = new MovimentacaoEstoque
        {
            EstoqueId = estoqueId,
            UsuarioId = usuarioId,
            TipoMovimentacao = TipoMovimentacaoEstoque.Ajuste,
            Quantidade = quantidade,
            Motivo = $"Estorno do Pedido ID {pedidoId}. Motivo: {motivo}",
            CriadoEm = DateTime.UtcNow
        };

        new MovimentacaoEstoqueValidator().ValidaOuLancaExcecao(mov);
        return mov;
    }

    public void ValidarPodeCancelar(Pedido? pedido, int pedidoId) =>
         _ = pedido ?? throw new DomainException($"Pedido ID {pedidoId} não encontrado.");

    public void ValidarUsuarioResponsavel(Usuario? usuario) =>
         _ = usuario ?? throw new DomainException("Usuário responsável inválido ou inativo.");
}
