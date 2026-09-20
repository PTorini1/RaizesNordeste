using Microsoft.AspNetCore.Mvc;
using RaizesNordeste.Application.Common;
using RaizesNordesteApi.Extensions;

namespace RaizesNordesteApi.Handlers;

public class ResultadoHttpHandler
{
    public ActionResult Tratar(
        ControllerBase controller,
        ResultadoOperacao resultado,
        string? createdAction = null,
        object? routeValues = null)
    {
        return resultado.Status switch
        {
            StatusOperacao.SemConteudo => controller.NoContent(),
            StatusOperacao.Criado => CriarResultadoCriado<object>(controller, null, createdAction, routeValues),
            StatusOperacao.Sucesso => controller.Ok(),
            _ => CriarErro(controller, resultado)
        };
    }

    public ActionResult<T> Tratar<T>(
        ControllerBase controller,
        ResultadoOperacao<T> resultado,
        string? createdAction = null,
        object? routeValues = null)
    {
        return resultado.Status switch
        {
            StatusOperacao.Sucesso => controller.Ok(resultado.Valor),
            StatusOperacao.Criado => CriarResultadoCriado(controller, resultado.Valor, createdAction, routeValues),
            StatusOperacao.SemConteudo => controller.NoContent(),
            _ => CriarErro(controller, resultado)
        };
    }

    private static ActionResult CriarResultadoCriado<T>(
        ControllerBase controller,
        T? valor,
        string? createdAction,
        object? routeValues)
    {
        return string.IsNullOrWhiteSpace(createdAction)
            ? controller.Created(string.Empty, valor)
            : controller.CreatedAtAction(createdAction, routeValues, valor);
    }

    private static ActionResult CriarErro(ControllerBase controller, ResultadoOperacao resultado)
    {
        var statusCode = ObterStatusCode(resultado.Status);
        var erro = FabricaErroApi.Criar(
            controller.HttpContext,
            statusCode,
            resultado.Codigo ?? "ERRO",
            resultado.Mensagem ?? "A operacao nao pode ser concluida.",
            resultado.Erros);

        return controller.StatusCode(statusCode, erro);
    }

    private static int ObterStatusCode(StatusOperacao status)
    {
        return status switch
        {
            StatusOperacao.BadRequest => StatusCodes.Status400BadRequest,
            StatusOperacao.NaoEncontrado => StatusCodes.Status404NotFound,
            StatusOperacao.Proibido => StatusCodes.Status403Forbidden,
            StatusOperacao.Conflito => StatusCodes.Status409Conflict,
            StatusOperacao.Validacao => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
