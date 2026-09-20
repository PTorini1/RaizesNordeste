using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesNordeste.Application.Common;
using RaizesNordeste.Application.UseCases.Seed;
using RaizesNordesteApi.Handlers;

namespace RaizesNordesteApi.Controllers;

[ApiController]
[Route("api/carga-inicial")]
[Produces("application/json")]
public class CargaInicialController(
    SeedDatabaseUseCase seedDatabaseUseCase,
    ResultadoHttpHandler resultadoHttpHandler) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<object>> ExecutarCargaInicial(
        [FromQuery] bool reset = false, CancellationToken cancellationToken = default)
    {
        var response = await seedDatabaseUseCase.ExecutarAsync(reset, cancellationToken);
        return resultadoHttpHandler.Tratar(this, ResultadoOperacao<object>.Sucesso(response));
    }
}
