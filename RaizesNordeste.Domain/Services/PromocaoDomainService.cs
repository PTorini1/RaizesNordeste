using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Validators;

namespace RaizesNordeste.Domain.Services;

public class PromocaoDomainService
{
    private readonly PromocaoValidator _validator = new();

    public Promocao CriarPromocao(
        string nome,
        string? descricao,
        string tipoDesconto,
        decimal valorDesconto,
        DateTime inicioVigencia,
        DateTime fimVigencia,
        int? produtoId,
        CanalPedido? canalPedido,
        string? perfilCliente)
    {
        var promocao = new Promocao
        {
            Nome = nome,
            Descricao = descricao,
            TipoDesconto = tipoDesconto,
            ValorDesconto = valorDesconto,
            InicioVigencia = inicioVigencia,
            FimVigencia = fimVigencia,
            Ativa = true,
            ProdutoId = produtoId,
            CanalPedido = canalPedido,
            PerfilCliente = perfilCliente
        };

        _validator.ValidaOuLancaExcecao(promocao);
        return promocao;
    }

    public (decimal DescontoTotal, Promocao? PromocaoAplicada) CalcularDesconto(
        Pedido pedido,
        IReadOnlyCollection<Promocao> promocoesAtivas)
    {
        var valorBase = pedido.Itens.Sum(i => i.ValorTotal);
        decimal descontoTotal = 0;
        Promocao? promocaoAplicada = null;

        foreach (var item in pedido.Itens)
        {
            var promocaoProduto = promocoesAtivas.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);
            if (promocaoProduto is null)
            {
                continue;
            }

            var descontoItem = CalcularDescontoItem(promocaoProduto, item.ValorTotal, item.Quantidade);
            descontoTotal += Math.Min(descontoItem, item.ValorTotal);
            promocaoAplicada = promocaoProduto;
        }

        if (descontoTotal == 0)
        {
            var promocaoCanal = promocoesAtivas.FirstOrDefault(p => p.CanalPedido == pedido.CanalPedido && p.ProdutoId == null);
            if (promocaoCanal is not null)
            {
                descontoTotal = CalcularDescontoItem(promocaoCanal, valorBase, 1);
                promocaoAplicada = promocaoCanal;
            }
        }

        descontoTotal = Math.Min(descontoTotal, valorBase);
        return (descontoTotal, promocaoAplicada);
    }

    private static decimal CalcularDescontoItem(Promocao promocao, decimal valor, int quantidade)
    {
        if (promocao.TipoDesconto.Equals("PERCENTUAL", StringComparison.OrdinalIgnoreCase))
        {
            return valor * promocao.ValorDesconto / 100m;
        }

        if (promocao.TipoDesconto.Equals("VALOR", StringComparison.OrdinalIgnoreCase))
        {
            return promocao.ValorDesconto * quantidade;
        }

        return 0;
    }
}
