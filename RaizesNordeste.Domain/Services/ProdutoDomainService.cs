using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Validators;

namespace RaizesNordeste.Domain.Services;

public class ProdutoDomainService
{
    public Produto CriarProduto(string nome, string? descricao, decimal precoBase, string categoria, bool sazonal, bool ativo)
    {
        var produto = new Produto
        {
            Nome = nome,
            Descricao = descricao,
            PrecoBase = precoBase,
            Categoria = categoria,
            Sazonal = sazonal,
            Ativo = ativo
        };

        new ProdutoValidator().ValidaOuLancaExcecao(produto);
        return produto;
    }

    public void AtualizarProduto(Produto produto, string nome, string? descricao, decimal precoBase, string categoria, bool sazonal, bool ativo)
    {
        produto.Nome = nome;
        produto.Descricao = descricao;
        produto.PrecoBase = precoBase;
        produto.Categoria = categoria;
        produto.Sazonal = sazonal;
        produto.Ativo = ativo;

        new ProdutoValidator().ValidaOuLancaExcecao(produto);
    }

    public void AtualizarParcial(Produto produto, string? nome, string? descricao, decimal? precoBase, string? categoria, bool? sazonal, bool? ativo)
    {
        produto.Nome = nome ?? produto.Nome;
        produto.Descricao = descricao ?? produto.Descricao;
        produto.PrecoBase = precoBase ?? produto.PrecoBase;
        produto.Categoria = categoria ?? produto.Categoria;
        produto.Sazonal = sazonal ?? produto.Sazonal;
        produto.Ativo = ativo ?? produto.Ativo;

        new ProdutoValidator().ValidaOuLancaExcecao(produto);
    }

    public void ValidarAtivo(Produto? produto)
    {
        if (produto is null || !produto.Ativo)
        {
            throw new DomainException("Produto inválido ou inativo.");
        }
    }
}
