namespace RaizesNordesteApi.Contracts.Requests;

public class ConsultaPaginacao
{
    private const int PaginaPadrao = 1;
    private const int LimitePadrao = 10;
    private const int LimiteMaximo = 100;

    public int Pagina { get; set; } = PaginaPadrao;
    public int Limite { get; set; } = LimitePadrao;

    public int PaginaNormalizada => Pagina < PaginaPadrao ? PaginaPadrao : Pagina;

    public int LimiteNormalizado => Limite switch
    {
        < 1 => LimitePadrao,
        > LimiteMaximo => LimiteMaximo,
        _ => Limite
    };
}
