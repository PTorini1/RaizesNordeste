using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesNordeste.Application.DTOs.Pedidos;
using RaizesNordeste.Application.UseCases.Pedidos;
using RaizesNordeste.Domain.Enums;
using RaizesNordesteApi.Contracts.Requests;
using RaizesNordesteApi.Contracts.Responses;
using RaizesNordesteApi.Extensions;
using RaizesNordesteApi.Handlers;

namespace RaizesNordesteApi.Controllers;

[ApiController]
[Route("api/pedido")]
[Produces("application/json")]
[Authorize]
public class PedidoController(
    CriarPedidoUseCase criarPedidoUseCase,
    AtualizarStatusPedidoUseCase atualizarStatusPedidoUseCase,
    CancelarPedidoUseCase cancelarPedidoUseCase,
    ObterPedidoPorIdUseCase obterPedidoPorIdUseCase,
    ListarPedidosUseCase listarPedidosUseCase,
    ResultadoHttpHandler resultadoHttpHandler)
    : ControllerBase
{
    /// <summary>
    /// Cria um novo pedido com base nas informações fornecidas na requisição. O pedido é associado ao usuário autenticado e processado de acordo com as regras de negócio definidas. Se a criação for bem-sucedida, o pedido criado é retornado na resposta, caso contrário, uma resposta de erro apropriada é gerada.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>O pedido criado com sucesso.</returns>
    [HttpPost]
    [Authorize(Roles = "Cliente,Atendente")]
    public async Task<ActionResult<PedidoResponse>> CriarPedido(
        [FromBody] CriarPedidoRequest request, CancellationToken cancellationToken)
    {
        var resultado = await criarPedidoUseCase.ExecutarAsync(request, User.ObterUsuarioAutenticado(), cancellationToken);
        return resultadoHttpHandler.Tratar(this, resultado, nameof(ObterPedidoPorId), new { id = resultado.Valor?.Id });
    }

    /// <summary>
    /// Obtém os detalhes de um pedido específico com base no ID fornecido. O acesso a essa informação é restrito ao usuário que criou o pedido ou a usuários com permissões administrativas. Se o pedido for encontrado e o usuário tiver permissão para acessá-lo, os detalhes do pedido são retornados na resposta. Caso contrário, uma resposta de erro apropriada é gerada, indicando se o pedido não foi encontrado ou se o acesso foi negado.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Os detalhes do pedido correspondente ao ID informado.</returns>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Gerente,Cliente")]
    public async Task<ActionResult<PedidoResponse>> ObterPedidoPorId(
        [FromRoute] int id, CancellationToken cancellationToken)
    {
        var resultado = await obterPedidoPorIdUseCase.ExecutarAsync(id, User.ObterUsuarioAutenticado(), cancellationToken);
        return resultadoHttpHandler.Tratar(this, resultado);
    }


    /// <summary>
    /// Lista os pedidos, podendo ser filtrados por canal de pedido (aplicativo, telefone, presencial). O acesso a essa informação é restrito a usuários com permissões administrativas ou de atendimento. Os resultados são paginados para facilitar a navegação e a visualização dos pedidos. Se os pedidos forem encontrados e o usuário tiver permissão para acessá-los, uma lista paginada de pedidos é retornada na resposta. Caso contrário, uma resposta de erro apropriada é gerada, indicando se nenhum pedido foi encontrado ou se o acesso foi negado.
    /// </summary>
    /// <param name="canalPedido"></param>
    /// <param name="paginacao"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Uma resposta paginada contendo a lista de pedidos.</returns>
    [HttpGet]
    [Authorize(Roles = "Admin,Gerente")]
    public async Task<ActionResult<RespostaPaginada<PedidoResponse>>> ListarPedido(
        [FromQuery] CanalPedido? canalPedido, [FromQuery] ConsultaPaginacao paginacao, CancellationToken cancellationToken)
    {
        var response = await listarPedidosUseCase.ExecutarAsync(canalPedido, cancellationToken);
        return resultadoHttpHandler.Tratar(this, RaizesNordeste.Application.Common.ResultadoOperacao<RespostaPaginada<PedidoResponse>>.Sucesso(response.ParaRespostaPaginada(paginacao)));
    }

    /// <summary>
    /// Atualiza o status de um pedido específico com base no ID fornecido e no novo status desejado. O acesso a essa operação é restrito a usuários com permissões administrativas ou de atendimento. Se o pedido for encontrado, o status for atualizado com sucesso e o usuário tiver permissão para realizar a atualização, os detalhes do pedido atualizado são retornados na resposta. Caso contrário, uma resposta de erro apropriada é gerada, indicando se o pedido não foi encontrado, se a atualização falhou ou se o acesso foi negado.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="novoStatus"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Os detalhes do pedido com o status atualizado.</returns>
    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Admin,Gerente,Atendente")]
    public async Task<ActionResult<PedidoResponse>> AtualizarStatus(
        [FromRoute] int id, [FromBody] StatusPedido novoStatus, CancellationToken cancellationToken)
    {
        var response = await atualizarStatusPedidoUseCase.ExecutarAsync(id, novoStatus, cancellationToken);
        return resultadoHttpHandler.Tratar(this, RaizesNordeste.Application.Common.ResultadoOperacao<PedidoResponse>.Sucesso(response));
    }

    /// <summary>
    /// Cancela um pedido específico com base no ID fornecido, motivo de cancelamento e ID do usuário que está solicitando o cancelamento. O acesso a essa operação é restrito a usuários com permissões administrativas ou de atendimento. Se o pedido for encontrado, o cancelamento for realizado com sucesso e o usuário tiver permissão para realizar o cancelamento, os detalhes do pedido cancelado são retornados na resposta. Caso contrário, uma resposta de erro apropriada é gerada, indicando se o pedido não foi encontrado, se o cancelamento falhou ou se o acesso foi negado.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Os detalhes do pedido cancelado.</returns>
    [HttpPost("{id:int}/cancelar")]
    [Authorize(Roles = "Admin,Gerente,Atendente")]
    public async Task<ActionResult<PedidoResponse>> CancelarPedido(
        [FromRoute] int id, [FromBody] CancelarPedidoRequest request, CancellationToken cancellationToken)
    {
        var response = await cancelarPedidoUseCase.ExecutarAsync(id, request.UsuarioId, request.Motivo, cancellationToken);
        return resultadoHttpHandler.Tratar(this, RaizesNordeste.Application.Common.ResultadoOperacao<PedidoResponse>.Sucesso(response));
    }
}
