using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesNordeste.Application.Common;
using RaizesNordeste.Application.UseCases.Auditorias;
using RaizesNordeste.Domain.Entities;
using RaizesNordesteApi.Contracts.Requests;
using RaizesNordesteApi.Contracts.Responses;
using RaizesNordesteApi.Extensions;
using RaizesNordesteApi.Handlers;

namespace RaizesNordesteApi.Controllers;

[ApiController]
[Route("api/auditoria")]
[Produces("application/json")]
[Authorize(Roles = "Admin")]
public class AuditoriaController(
    ListarAuditoriasUseCase listarAuditoriasUseCase,
    ListarAuditoriasPorEntidadeUseCase listarAuditoriasPorEntidadeUseCase,
    ResultadoHttpHandler resultadoHttpHandler)
    : ControllerBase
{
    /// <summary>
    /// Lista os logs de auditoria, podendo ser filtrados por entidade e ação. Os resultados são paginados.
    /// </summary>
    /// <param name="entidade"></param>
    /// <param name="acao"></param>
    /// <param name="paginacao"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A lista paginada de logs de auditoria correspondentes aos filtros.</returns>
    [HttpGet]
    public async Task<ActionResult<RespostaPaginada<Auditoria>>> ListarAuditoria(
        [FromQuery] string? entidade,
        [FromQuery] string? acao,
        [FromQuery] ConsultaPaginacao paginacao,
        CancellationToken cancellationToken)
    {
        var logs = await listarAuditoriasUseCase.ExecutarAsync(entidade, acao, cancellationToken);
        return resultadoHttpHandler.Tratar(this, ResultadoOperacao<RespostaPaginada<Auditoria>>.Sucesso(logs.ParaRespostaPaginada(paginacao)));
    }

    /// <summary>
    /// Lista os logs de auditoria para uma entidade específica, podendo ser filtrados por ação. Os resultados são paginados.
    /// </summary>
    /// <param name="entidade"></param>
    /// <param name="paginacao"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A lista paginada de logs de auditoria para a entidade especificada.</returns>
    [HttpGet("entidade/{entidade}")]
    public async Task<ActionResult<RespostaPaginada<Auditoria>>> ListarPorEntidade(
        [FromRoute] string entidade,
        [FromQuery] ConsultaPaginacao paginacao,
        CancellationToken cancellationToken)
    {
        var logs = await listarAuditoriasPorEntidadeUseCase.ExecutarAsync(entidade, cancellationToken);
        return resultadoHttpHandler.Tratar(this, ResultadoOperacao<RespostaPaginada<Auditoria>>.Sucesso(logs.ParaRespostaPaginada(paginacao)));
    }
}
