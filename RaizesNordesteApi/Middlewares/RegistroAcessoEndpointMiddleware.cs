using System.Diagnostics;

namespace RaizesNordesteApi.Middlewares;

public class RegistroAcessoEndpointMiddleware(
    RequestDelegate next,
    ILogger<RegistroAcessoEndpointMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var metodo = context.Request.Method;
        var caminho = context.Request.Path.Value ?? "/";
        var consulta = context.Request.QueryString.HasValue
            ? context.Request.QueryString.Value
            : string.Empty;

        logger.LogInformation("Endpoint acessado: {Metodo} {Caminho}{Consulta}", metodo, caminho, consulta);

        try
        {
            await next(context);

            stopwatch.Stop();

            logger.LogInformation(
                "Endpoint concluído: {Metodo} {Caminho}{Consulta} respondeu {StatusCode} em {TempoDecorridoMs} ms",
                metodo,
                caminho,
                consulta,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
        catch (Exception exception)
        {
            stopwatch.Stop();

            logger.LogError(
                exception,
                "Endpoint falhou: {Metodo} {Caminho}{Consulta} após {TempoDecorridoMs} ms",
                metodo,
                caminho,
                consulta,
                stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}
