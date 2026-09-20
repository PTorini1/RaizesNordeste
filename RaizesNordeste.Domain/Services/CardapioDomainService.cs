using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Validators;

namespace RaizesNordeste.Domain.Services;

public class CardapioDomainService
{
    private readonly CardapioUnidadeValidator _validator = new();

    public void ValidarAssociacao(Unidade? unidade, Produto? produto, IEnumerable<CardapioUnidade> cardapio, int produtoId)
    {
        if (unidade is null || !unidade.Ativa)
        {
            throw new DomainException("Unidade inválida ou inativa.");
        }

        if (produto is null || !produto.Ativo)
        {
            throw new DomainException("Produto inválido ou inativo.");
        }

        if (cardapio.Any(c => c.ProdutoId == produtoId))
        {
            throw new DomainException("Este produto já está associado ao cardápio desta unidade.");
        }
    }

    public CardapioUnidade CriarAssociacao(
        int unidadeId,
        int produtoId,
        decimal preco,
        bool disponivel,
        DateTime? inicioVigencia,
        DateTime? fimVigencia)
    {
        var cardapio = new CardapioUnidade
        {
            UnidadeId = unidadeId,
            ProdutoId = produtoId,
            Preco = preco,
            Disponivel = disponivel,
            InicioVigencia = inicioVigencia,
            FimVigencia = fimVigencia
        };

        _validator.ValidaOuLancaExcecao(cardapio);
        return cardapio;
    }

    public CardapioUnidade ValidarItemExistente(IEnumerable<CardapioUnidade> cardapio, int produtoId, int unidadeId)
    {
        var item = cardapio.FirstOrDefault(c => c.ProdutoId == produtoId);
        _ = item ?? throw new DomainException($"O produto ID {produtoId} não está associado à unidade ID {unidadeId}.");

        return item;
    }

    public void AtualizarAssociacao(
        CardapioUnidade cardapio,
        decimal preco,
        bool disponivel,
        DateTime? inicioVigencia,
        DateTime? fimVigencia)
    {
        cardapio.Preco = preco;
        cardapio.Disponivel = disponivel;
        cardapio.InicioVigencia = inicioVigencia;
        cardapio.FimVigencia = fimVigencia;

        _validator.ValidaOuLancaExcecao(cardapio);
    }
}
