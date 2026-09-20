using RaizesNordesteApi.Contracts.Responses;

namespace RaizesNordesteApi.Extensions;

public static class FabricaErroApi
{
    public static RespostaErroApi Criar(
        HttpContext httpContext,
        int status,
        string codigo,
        string mensagem,
        IReadOnlyDictionary<string, string[]>? erros = null)
    {
        return new RespostaErroApi
        {
            Status = status,
            Codigo = codigo,
            Mensagem = mensagem,
            RastreamentoId = httpContext.TraceIdentifier,
            Caminho = httpContext.Request.Path,
            DataHora = DateTimeOffset.UtcNow,
            Erros = erros
        };
    }
}
