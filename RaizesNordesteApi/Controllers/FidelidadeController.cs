using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesNordeste.Application.DTOs.Fidelidade;
using RaizesNordeste.Application.UseCases.Fidelidade;
using RaizesNordesteApi.Extensions;
using RaizesNordesteApi.Handlers;

namespace RaizesNordesteApi.Controllers;

[ApiController]
[Route("api/fidelidade")]
[Produces("application/json")]
[Authorize]
public class FidelidadeController(
    AderirFidelidadeUseCase aderirFidelidadeUseCase,
    ResgatarPontosUseCase resgatarPontosUseCase,
    ObterSaldoFidelidadeUseCase obterSaldoFidelidadeUseCase,
    ResultadoHttpHandler resultadoHttpHandler)
    : ControllerBase
{
    /// <summary>
    /// Permite que um cliente adira ao programa de fidelidade, criando uma conta de fidelidade associada ao cliente. O cliente passa a acumular pontos a cada compra realizada, podendo posteriormente resgatar esses pontos por recompensas ou descontos. Essa operação é essencial para incentivar a fidelização dos clientes e promover a participação no programa de fidelidade.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A conta de fidelidade criada.</returns>
    [HttpPost("adesao")]
    [Authorize(Roles = "Cliente")]
    public async Task<ActionResult<ContaFidelidadeResponse>> AderirFidelidade(
        [FromBody] AderirFidelidadeRequest request, CancellationToken cancellationToken)
    {
        var resultado = await aderirFidelidadeUseCase.ExecutarAsync(request, User.ObterUsuarioAutenticado(), cancellationToken);
        return resultadoHttpHandler.Tratar(this, resultado);
    }

    /// <summary>
    /// Permite que um cliente resgate seus pontos acumulados no programa de fidelidade por recompensas ou descontos. O cliente pode escolher entre as opções disponíveis para resgatar seus pontos, e a operação é processada para atualizar o saldo de pontos do cliente e conceder a recompensa correspondente. Essa funcionalidade é fundamental para incentivar os clientes a participarem ativamente do programa de fidelidade e aproveitarem os benefícios oferecidos.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A conta de fidelidade atualizada após o resgate.</returns>
    [HttpPost("resgate")]
    [Authorize(Roles = "Cliente")]
    public async Task<ActionResult<ContaFidelidadeResponse>> ResgatarPontos(
        [FromBody] ResgatarPontosRequest request, CancellationToken cancellationToken)
    {
        var resultado = await resgatarPontosUseCase.ExecutarAsync(request, User.ObterUsuarioAutenticado(), cancellationToken);
        return resultadoHttpHandler.Tratar(this, resultado);
    }

    /// <summary>
    /// Permite que um cliente consulte o saldo de pontos acumulados em sua conta de fidelidade, fornecendo informações atualizadas sobre a quantidade de pontos disponíveis para resgate. O cliente pode verificar seu saldo a qualquer momento para acompanhar seu progresso no programa de fidelidade e planejar futuros resgates. Essa funcionalidade é importante para manter os clientes informados sobre seus benefícios e incentivar a participação contínua no programa de fidelidade.
    /// </summary>
    /// <param name="clienteId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A conta de fidelidade contendo o saldo atual do cliente.</returns>
    [HttpGet("cliente/{clienteId:int}/saldo")]
    [Authorize(Roles = "Admin,Cliente")]
    public async Task<ActionResult<ContaFidelidadeResponse>> ObterSaldoFidelidade(
        [FromRoute] int clienteId, CancellationToken cancellationToken)
    {
        var resultado = await obterSaldoFidelidadeUseCase.ExecutarAsync(clienteId, User.ObterUsuarioAutenticado(), cancellationToken);
        return resultadoHttpHandler.Tratar(this, resultado);
    }
}
