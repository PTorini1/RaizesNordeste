using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesNordeste.Application.Common;
using RaizesNordeste.Application.DTOs.Cardapio;
using RaizesNordeste.Application.UseCases.Cardapio;
using RaizesNordesteApi.Contracts.Requests;
using RaizesNordesteApi.Contracts.Responses;
using RaizesNordesteApi.Extensions;
using RaizesNordesteApi.Handlers;

namespace RaizesNordesteApi.Controllers;

[ApiController]
[Route("api/unidade")]
[Produces("application/json")]
[Authorize]
public class CardapioController(
    ConsultarCardapioUnidadeUseCase consultarCardapioUseCase,
    AssociarProdutoCardapioUseCase associarProdutoCardapioUseCase,
    AtualizarProdutoCardapioUseCase atualizarProdutoCardapioUseCase,
    RemoverProdutoCardapioUseCase removerProdutoCardapioUseCase,
    ResultadoHttpHandler resultadoHttpHandler)
    : ControllerBase
{
    /// <summary>
    /// Obtém o cardápio de uma unidade específica, listando os produtos disponíveis. Os resultados são paginados.
    /// </summary>
    /// <param name="unidadeId"></param>
    /// <param name="paginacao"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Uma resposta paginada contendo o cardápio da unidade.</returns>
    [HttpGet("{unidadeId:int}/cardapio")]
    [Authorize(Roles = "Admin,Gerente,Cliente")]
    public async Task<ActionResult<RespostaPaginada<CardapioResponse>>> ObterCardapioUnidade(
        [FromRoute] int unidadeId, [FromQuery] ConsultaPaginacao paginacao, CancellationToken cancellationToken)
    {
        var response = await consultarCardapioUseCase.ExecutarAsync(unidadeId, cancellationToken);
        return resultadoHttpHandler.Tratar(this, ResultadoOperacao<RespostaPaginada<CardapioResponse>>.Sucesso(response.ParaRespostaPaginada(paginacao)));
    }

    /// <summary>
    /// Associa um produto ao cardápio de uma unidade específica, tornando-o disponível para os clientes. O produto é adicionado ao cardápio da unidade e pode ser posteriormente atualizado ou removido conforme necessário.
    /// </summary>
    /// <param name="unidadeId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>O item do cardápio criado.</returns>
    [HttpPost("{unidadeId:int}/cardapio")]
    [Authorize(Roles = "Admin,Gerente")]
    public async Task<ActionResult<CardapioResponse>> AssociarProduto(
        [FromRoute] int unidadeId, [FromBody] AssociarProdutoRequest request, CancellationToken cancellationToken)
    {
        var response = await associarProdutoCardapioUseCase.ExecutarAsync(unidadeId, request, cancellationToken);
        return resultadoHttpHandler.Tratar(this, ResultadoOperacao<CardapioResponse>.Criado(response), nameof(ObterCardapioUnidade), new { unidadeId });
    }

    /// <summary>
    /// Atualiza as informações de um produto no cardápio de uma unidade específica, permitindo modificar detalhes como preço, descrição ou disponibilidade. Essa operação é útil para manter o cardápio atualizado e refletir mudanças nos produtos oferecidos pela unidade.
    /// </summary>
    /// <param name="unidadeId"></param>
    /// <param name="produtoId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>O item do cardápio atualizado.</returns>
    [HttpPut("{unidadeId:int}/cardapio/{produtoId:int}")]
    [Authorize(Roles = "Admin,Gerente")]
    public async Task<ActionResult<CardapioResponse>> AtualizarProdutoCardapio(
        [FromRoute] int unidadeId, [FromRoute] int produtoId,
        [FromBody] AtualizarProdutoCardapioRequest request, CancellationToken cancellationToken)
    {
        var response = await atualizarProdutoCardapioUseCase.ExecutarAsync(unidadeId, produtoId, request, cancellationToken);
        return resultadoHttpHandler.Tratar(this, ResultadoOperacao<CardapioResponse>.Sucesso(response));
    }

    /// <summary>
    /// Remove um produto do cardápio de uma unidade específica, tornando-o indisponível para os clientes. Essa operação é útil para gerenciar o cardápio e garantir que apenas os produtos atualmente oferecidos pela unidade estejam visíveis para os clientes.
    /// </summary>
    /// <param name="unidadeId"></param>
    /// <param name="produtoId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Sem conteúdo em caso de sucesso.</returns>
    [HttpDelete("{unidadeId:int}/cardapio/{produtoId:int}")]
    [Authorize(Roles = "Admin,Gerente")]
    public async Task<IActionResult> RemoverProdutoCardapio(
        [FromRoute] int unidadeId, [FromRoute] int produtoId, CancellationToken cancellationToken)
    {
        await removerProdutoCardapioUseCase.ExecutarAsync(unidadeId, produtoId, cancellationToken);
        return resultadoHttpHandler.Tratar(this, ResultadoOperacao.SemConteudo());
    }
}
