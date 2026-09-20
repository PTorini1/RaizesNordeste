using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesNordeste.Application.Common;
using RaizesNordeste.Application.DTOs.Estoque;
using RaizesNordeste.Application.UseCases.Estoques;
using RaizesNordesteApi.Contracts.Requests;
using RaizesNordesteApi.Contracts.Responses;
using RaizesNordesteApi.Extensions;
using RaizesNordesteApi.Handlers;

namespace RaizesNordesteApi.Controllers;

[ApiController]
[Route("api/estoque")]
[Produces("application/json")]
[Authorize]
public class EstoqueController(
    ConsultarEstoqueUnidadeUseCase consultarEstoqueUnidadeUseCase,
    ConsultarEstoqueProdutoUseCase consultarEstoqueProdutoUseCase,
    RegistrarEntradaEstoqueUseCase registrarEntradaEstoqueUseCase,
    RegistrarSaidaEstoqueUseCase registrarSaidaEstoqueUseCase,
    ResultadoHttpHandler resultadoHttpHandler)
    : ControllerBase
{
    /// <summary>
    /// Obtém o estoque de uma unidade específica, listando os produtos disponíveis e suas quantidades. Os resultados são paginados para facilitar a navegação em grandes volumes de dados. Essa operação é útil para gerentes e administradores monitorarem o estoque de cada unidade e tomarem decisões informadas sobre reposição e gerenciamento de inventário.
    /// </summary>
    /// <param name="unidadeId"></param>
    /// <param name="paginacao"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Uma resposta paginada contendo o estoque da unidade.</returns>
    [HttpGet("unidade/{unidadeId:int}")]
    [Authorize(Roles = "Admin,Gerente")]
    public async Task<ActionResult<RespostaPaginada<EstoqueResponse>>> ConsultarEstoqueUnidade(
        [FromRoute] int unidadeId, [FromQuery] ConsultaPaginacao paginacao, CancellationToken cancellationToken)
    {
        var response = await consultarEstoqueUnidadeUseCase.ExecutarAsync(unidadeId, cancellationToken);
        return resultadoHttpHandler.Tratar(this, ResultadoOperacao<RespostaPaginada<EstoqueResponse>>.Sucesso(response.ParaRespostaPaginada(paginacao)));
    }

    /// <summary>
    /// Obtém o estoque de um produto específico em uma unidade específica, retornando a quantidade disponível. Essa operação é útil para gerentes e administradores verificarem rapidamente a disponibilidade de um produto em uma unidade específica, facilitando a tomada de decisões sobre reposição e gerenciamento de inventário. Se o registro de estoque para o produto e unidade especificados não for encontrado, uma resposta de "Não Encontrado" será retornada. Caso contrário, a quantidade disponível do produto na unidade será retornada com sucesso.
    /// </summary>
    /// <param name="unidadeId"></param>
    /// <param name="produtoId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>O estoque do produto correspondente ao ID informado.</returns>
    [HttpGet("unidade/{unidadeId:int}/produto/{produtoId:int}")]
    [Authorize(Roles = "Admin,Gerente")]
    public async Task<ActionResult<EstoqueResponse>> ConsultarEstoqueProduto(
        [FromRoute] int unidadeId, [FromRoute] int produtoId, CancellationToken cancellationToken)
    {
        var response = await consultarEstoqueProdutoUseCase.ExecutarAsync(unidadeId, produtoId, cancellationToken);

        var resultado = response is null
            ? ResultadoOperacao<EstoqueResponse>.NaoEncontrado($"Registro de estoque não encontrado para o produto ID {produtoId} na unidade ID {unidadeId}.")
            : ResultadoOperacao<EstoqueResponse>.Sucesso(response);

        return resultadoHttpHandler.Tratar(this, resultado);
    }

    /// <summary>
    /// Registra uma entrada de estoque para um produto em uma unidade específica, aumentando a quantidade disponível. Essa operação é útil para gerentes e administradores atualizarem o estoque quando novos produtos chegam ou quando há necessidade de corrigir quantidades. O registro de entrada inclui informações como a quantidade adicionada, a data da entrada e o motivo da atualização, permitindo um controle detalhado do histórico de movimentações de estoque.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>O registro de estoque atualizado após a entrada.</returns>
    [HttpPost("entrada")]
    [Authorize(Roles = "Admin,Gerente")]
    public async Task<ActionResult<EstoqueResponse>> RegistrarEntrada(
        [FromBody] EntradaEstoqueRequest request, CancellationToken cancellationToken)
    {
        var response = await registrarEntradaEstoqueUseCase.ExecutarAsync(request, cancellationToken);
        return resultadoHttpHandler.Tratar(this, ResultadoOperacao<EstoqueResponse>.Criado(response));
    }

    /// <summary>
    /// Registra uma saída de estoque para um produto em uma unidade específica, diminuindo a quantidade disponível. Essa operação é útil para gerentes e administradores atualizarem o estoque quando produtos são vendidos ou utilizados, garantindo que as quantidades refletidas no sistema estejam sempre atualizadas. O registro de saída inclui informações como a quantidade removida, a data da saída e o motivo da atualização, permitindo um controle detalhado do histórico de movimentações de estoque. Se a quantidade solicitada para saída exceder a quantidade disponível em estoque, uma resposta de "Bad Request" será retornada, indicando que a operação não pode ser concluída devido à falta de estoque suficiente.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>O registro de estoque atualizado após a saída.</returns>
    [HttpPost("saida")]
    [Authorize(Roles = "Admin,Gerente,Atendente")]
    public async Task<ActionResult<EstoqueResponse>> RegistrarSaida(
        [FromBody] SaidaEstoqueRequest request, CancellationToken cancellationToken)
    {
        var response = await registrarSaidaEstoqueUseCase.ExecutarAsync(request, cancellationToken);
        return resultadoHttpHandler.Tratar(this, ResultadoOperacao<EstoqueResponse>.Criado(response));
    }
}
