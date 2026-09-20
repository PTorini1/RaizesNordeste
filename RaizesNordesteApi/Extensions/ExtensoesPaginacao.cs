using RaizesNordesteApi.Contracts.Requests;
using RaizesNordesteApi.Contracts.Responses;

namespace RaizesNordesteApi.Extensions;

public static class ExtensoesPaginacao
{
    public static RespostaPaginada<T> ParaRespostaPaginada<T>(
        this IEnumerable<T> source,
        ConsultaPaginacao paginacao)
    {
        var pagina = paginacao.PaginaNormalizada;
        var limite = paginacao.LimiteNormalizado;
        var itens = source.ToList();
        var totalItens = itens.Count;
        var totalPaginas = totalItens == 0
            ? 0
            : (int)Math.Ceiling(totalItens / (double)limite);

        return new RespostaPaginada<T>
        {
            Pagina = pagina,
            Limite = limite,
            TotalItens = totalItens,
            TotalPaginas = totalPaginas,
            Itens = itens
                .Skip((pagina - 1) * limite)
                .Take(limite)
                .ToList()
        };
    }
}
