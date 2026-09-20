using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordesteApi.Extensions;

namespace RaizesNordesteApi.Filters;

public class ApiExceptionFilter(ILogger<ApiExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is DomainException domainException)
        {
            logger.LogWarning(context.Exception, "Regra de negócio violada na API.");

            var error = FabricaErroApi.Criar(
                context.HttpContext,
                StatusCodes.Status409Conflict,
                "REGRA_NEGOCIO_VIOLADA",
                domainException.Message);

            context.Result = new ObjectResult(error)
            {
                StatusCode = StatusCodes.Status409Conflict
            };
            context.ExceptionHandled = true;
            return;
        }

        logger.LogError(context.Exception, "Ocorreu uma exceção não tratada na API.");

        var genericError = FabricaErroApi.Criar(
            context.HttpContext,
            StatusCodes.Status500InternalServerError,
            "ERRO_INTERNO_SERVIDOR",
            "Ocorreu um erro inesperado no processamento da requisição.");

        context.Result = new ObjectResult(genericError)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
        context.ExceptionHandled = true;
    }
}
