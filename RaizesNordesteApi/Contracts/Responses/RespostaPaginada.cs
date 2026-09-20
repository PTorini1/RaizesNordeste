namespace RaizesNordesteApi.Contracts.Responses;

public class RespostaPaginada<T>
{
    public int Pagina { get; set; }
    public int Limite { get; set; }
    public int TotalItens { get; set; }
    public int TotalPaginas { get; set; }
    public IReadOnlyCollection<T> Itens { get; set; } = [];
}
