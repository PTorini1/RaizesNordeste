using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Exceptions;

namespace RaizesNordeste.Domain.Services;

public class EstoqueDomainService
{
    public int RegistrarMovimentacao(Estoque estoque, TipoMovimentacaoEstoque tipo, int quantidade)
    {
        if (quantidade <= 0)
        {
            throw new DomainException("A quantidade da movimentação deve ser maior que zero.");
        }

        var quantidadeAnterior = estoque.QuantidadeAtual;

        if (tipo is TipoMovimentacaoEstoque.Entrada or TipoMovimentacaoEstoque.Ajuste)
        {
            estoque.QuantidadeAtual += quantidade;
        }
        else if (tipo is TipoMovimentacaoEstoque.Saida or TipoMovimentacaoEstoque.Perda or TipoMovimentacaoEstoque.Venda)
        {
            BaixarEstoque(estoque, quantidade);
        }
        else
        {
            throw new DomainException("Tipo de movimentação de estoque inválido.");
        }

        estoque.AtualizadoEm = DateTime.UtcNow;
        return quantidadeAnterior;
    }

    public void BaixarEstoque(Estoque estoque, int quantidade)
    {
        if (quantidade <= 0)
        {
            throw new DomainException("A quantidade deve ser maior que zero.");
        }

        if (estoque.QuantidadeAtual < quantidade)
        {
            throw new DomainException($"Estoque insuficiente. Disponível: {estoque.QuantidadeAtual}, Requerido: {quantidade}.");
        }

        estoque.QuantidadeAtual -= quantidade;
        estoque.AtualizadoEm = DateTime.UtcNow;
    }

    public void ReporEstoque(Estoque estoque, int quantidade)
    {
        if (quantidade <= 0)
        {
            throw new DomainException("A quantidade deve ser maior que zero.");
        }

        estoque.QuantidadeAtual += quantidade;
        estoque.AtualizadoEm = DateTime.UtcNow;
    }

    public void ValidarSaida(TipoMovimentacaoEstoque tipo)
    {
        if (tipo == TipoMovimentacaoEstoque.Entrada)
        {
            throw new DomainException("Tipo de movimentação inválido para saída de estoque.");
        }
    }
}
