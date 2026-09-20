using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesNordeste.Application.DTOs.Pagamentos;
using RaizesNordeste.Application.UseCases.Pagamentos;
using RaizesNordesteApi.Extensions;
using RaizesNordesteApi.Handlers;

namespace RaizesNordesteApi.Controllers;

[ApiController]
[Route("api/pagamento")]
[Produces("application/json")]
[Authorize]
public class PagamentoController(
    SolicitarPagamentoUseCase solicitarPagamentoUseCase,
    ObterPagamentoPorIdUseCase obterPagamentoPorIdUseCase,
    ObterPagamentoPorPedidoIdUseCase obterPagamentoPorPedidoIdUseCase,
    ResultadoHttpHandler resultadoHttpHandler)
    : ControllerBase
{
    /// <summary>
    /// Efetua um pagamento para um pedido específico, processando as informações de pagamento fornecidas e associando o pagamento ao pedido correspondente. O processo de pagamento pode envolver a validação dos dados, a comunicação com gateways de pagamento e a atualização do status do pedido com base no resultado do pagamento. Essa operação é essencial
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>O pagamento processado com sucesso.</returns>
    [HttpPost]
    [Authorize(Roles = "Cliente,Atendente")]
    public async Task<ActionResult<PagamentoResponse>> EfetuarPagamento(
        [FromBody] SolicitarPagamentoRequest request, CancellationToken cancellationToken)
    {
        var resultado = await solicitarPagamentoUseCase.ExecutarAsync(request, User.ObterUsuarioAutenticado(), cancellationToken);
        return resultadoHttpHandler.Tratar(this, resultado, nameof(ObterPagamentoPorId), new { id = resultado.Valor?.Id });
    }

    /// <summary>
    /// Obtém os detalhes de um pagamento específico por meio do seu ID, permitindo que os usuários visualizem as informações relacionadas ao pagamento, como status, valor, método de pagamento e data de processamento. Essa operação é útil para acompanhar o histórico de pagamentos e verificar o status de transações específicas.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>O pagamento correspondente ao ID informado.</returns>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Gerente,Cliente")]
    public async Task<ActionResult<PagamentoResponse>> ObterPagamentoPorId(
        [FromRoute] int id, CancellationToken cancellationToken)
    {
        var resultado = await obterPagamentoPorIdUseCase.ExecutarAsync(id, User.ObterUsuarioAutenticado(), cancellationToken);
        return resultadoHttpHandler.Tratar(this, resultado);
    }

    /// <summary>
    /// Obtém os detalhes do pagamento associado a um pedido específico por meio do ID do pedido, permitindo que os usuários visualizem as informações relacionadas ao pagamento de um pedido, como status, valor, método de pagamento e data de processamento. Essa operação é útil para acompanhar o histórico de pagamentos relacionados a um pedido específico e verificar o status das transações associadas a esse pedido.
    /// </summary>
    /// <param name="pedidoId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>O pagamento associado ao ID do pedido.</returns>
    [HttpGet("pedido/{pedidoId:int}")]
    [Authorize(Roles = "Admin,Gerente,Cliente")]
    public async Task<ActionResult<PagamentoResponse>> ObterPagamentoPorPedidoId(
        [FromRoute] int pedidoId, CancellationToken cancellationToken)
    {
        var resultado = await obterPagamentoPorPedidoIdUseCase.ExecutarAsync(pedidoId, User.ObterUsuarioAutenticado(), cancellationToken);
        return resultadoHttpHandler.Tratar(this, resultado);
    }
}
