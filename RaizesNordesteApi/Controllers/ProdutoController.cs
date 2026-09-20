using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesNordeste.Application.Common;
using RaizesNordeste.Application.DTOs.Produtos;
using RaizesNordeste.Application.UseCases.Produtos;
using RaizesNordesteApi.Contracts.Requests;
using RaizesNordesteApi.Contracts.Responses;
using RaizesNordesteApi.Extensions;
using RaizesNordesteApi.Handlers;

namespace RaizesNordesteApi.Controllers;

[ApiController]
[Route("api/produto")]
[Produces("application/json")]
[Authorize]
public class ProdutoController(
    CriarProdutoUseCase criarProdutoUseCase,
    AtualizarProdutoUseCase atualizarProdutoUseCase,
    AtualizarParcialProdutoUseCase atualizarParcialProdutoUseCase,
    ObterProdutoPorIdUseCase obterProdutoPorIdUseCase,
    ListarProdutosUseCase listarProdutosUseCase,
    RemoverProdutoUseCase removerProdutoUseCase,
    ResultadoHttpHandler resultadoHttpHandler)
    : ControllerBase
{
    /// <summary>
    /// Cria um novo produto com as informações fornecidas. O produto é adicionado ao sistema e pode ser posteriormente listado, atualizado ou removido conforme necessário. Essa operação é restrita a usuários com as funções de Admin ou Gerente, garantindo que apenas usuários autorizados possam criar novos produtos no sistema.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>O produto criado com sucesso.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin,Gerente")]
    public async Task<ActionResult<ProdutoResponse>> CriarProduto(
        [FromBody] CriarProdutoRequest request, CancellationToken cancellationToken)
    {
        var response = await criarProdutoUseCase.ExecutarAsync(request, cancellationToken);
        return resultadoHttpHandler.Tratar(this, ResultadoOperacao<ProdutoResponse>.Criado(response), nameof(ObterProdutoPorId), new { id = response.Id });
    }

    /// <summary>
    /// Lista os produtos disponíveis, com opções de filtragem por status (ativos/inativos) e categoria. Os resultados são paginados para facilitar a navegação em grandes conjuntos de dados. Essa operação é acessível a usuários com as funções de Admin, Gerente, Atendente e Cliente, permitindo que uma ampla gama de usuários possa visualizar os produtos disponíveis no sistema.
    /// </summary>
    /// <param name="apenasAtivos"></param>
    /// <param name="categoria"></param>
    /// <param name="paginacao"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Uma resposta paginada contendo os produtos.</returns>
    [HttpGet]
    [Authorize(Roles = "Admin,Gerente,Atendente,Cliente")]
    public async Task<ActionResult<RespostaPaginada<ProdutoResponse>>> ListarProduto(
        [FromQuery] bool? apenasAtivos, [FromQuery] string? categoria,
        [FromQuery] ConsultaPaginacao paginacao, CancellationToken cancellationToken)
    {
        var response = await listarProdutosUseCase.ExecutarAsync(apenasAtivos, categoria, cancellationToken);
        return resultadoHttpHandler.Tratar(this, ResultadoOperacao<RespostaPaginada<ProdutoResponse>>.Sucesso(response.ParaRespostaPaginada(paginacao)));
    }

    /// <summary>
    /// Obtém os detalhes de um produto específico com base no seu ID. Se o produto for encontrado, suas informações são retornadas; caso contrário, uma resposta de "não encontrado" é retornada. Essa operação é acessível a usuários com as funções de Admin, Gerente, Atendente e Cliente, permitindo que uma ampla gama de usuários possa visualizar os detalhes dos produtos disponíveis no sistema.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>O produto com o ID especificado.</returns>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Gerente,Atendente,Cliente")]
    public async Task<ActionResult<ProdutoResponse>> ObterProdutoPorId(
        [FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await obterProdutoPorIdUseCase.ExecutarAsync(id, cancellationToken);

        var resultado = response is null
            ? ResultadoOperacao<ProdutoResponse>.NaoEncontrado($"Produto ID {id} não encontrado.")
            : ResultadoOperacao<ProdutoResponse>.Sucesso(response);

        return resultadoHttpHandler.Tratar(this, resultado);
    }

    /// <summary>
    /// Atualiza as informações de um produto existente com base no seu ID. As informações do produto são modificadas de acordo com os dados fornecidos na requisição. Se o produto for encontrado e atualizado com sucesso, as novas informações são retornadas; caso contrário, uma resposta de "não encontrado" é retornada. Essa operação é restrita a usuários com as funções de Admin ou Gerente, garantindo que apenas usuários autorizados possam modificar as informações dos produtos no sistema.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>O produto atualizado com as novas informações.</returns>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Gerente")]
    public async Task<ActionResult<ProdutoResponse>> AtualizarProduto(
        [FromRoute] int id, [FromBody] AtualizarProdutoRequest request, CancellationToken cancellationToken)
    {
        var response = await atualizarProdutoUseCase.ExecutarAsync(id, request, cancellationToken);

        var resultado = response is null
            ? ResultadoOperacao<ProdutoResponse>.NaoEncontrado($"Produto ID {id} não encontrado.")
            : ResultadoOperacao<ProdutoResponse>.Sucesso(response);

        return resultadoHttpHandler.Tratar(this, resultado);
    }

    /// <summary>
    /// Atualiza parcialmente as informações de um produto existente com base no seu ID. Permite modificar apenas os campos fornecidos na requisição, mantendo os demais campos inalterados. Se o produto for encontrado e atualizado com sucesso, as novas informações são retornadas; caso contrário, uma resposta de "não encontrado" é retornada. Essa operação é restrita a usuários com as funções de Admin ou Gerente, garantindo que apenas usuários autorizados possam modificar as informações dos produtos no sistema.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>O produto atualizado parcialmente.</returns>
    [HttpPatch("{id:int}")]
    [Authorize(Roles = "Admin,Gerente")]
    public async Task<ActionResult<ProdutoResponse>> AtualizarParcialProduto(
        [FromRoute] int id, [FromBody] AtualizarParcialProdutoRequest request, CancellationToken cancellationToken)
    {
        var response = await atualizarParcialProdutoUseCase.ExecutarAsync(id, request, cancellationToken);

        var resultado = response is null
            ? ResultadoOperacao<ProdutoResponse>.NaoEncontrado($"Produto ID {id} não encontrado.")
            : ResultadoOperacao<ProdutoResponse>.Sucesso(response);

        return resultadoHttpHandler.Tratar(this, resultado);
    }

    /// <summary>
    /// Remove um produto existente com base no seu ID. Se o produto for encontrado e removido com sucesso, uma resposta de "sem conteúdo" é retornada; caso contrário, uma resposta de "não encontrado" é retornada. Essa operação é restrita a usuários com as funções de Admin ou Gerente, garantindo que apenas usuários autorizados possam remover produtos do sistema.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Sem conteúdo em caso de sucesso.</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin,Gerente")]
    public async Task<IActionResult> RemoverProduto(
        [FromRoute] int id, CancellationToken cancellationToken)
    {
        var removido = await removerProdutoUseCase.ExecutarAsync(id, cancellationToken);

        var resultado = removido
            ? ResultadoOperacao.SemConteudo()
            : ResultadoOperacao.NaoEncontrado($"Produto ID {id} não encontrado.");

        return resultadoHttpHandler.Tratar(this, resultado);
    }
}
