using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesNordeste.Application.Common;
using RaizesNordeste.Application.DTOs.Unidades;
using RaizesNordeste.Application.UseCases.Unidades;
using RaizesNordesteApi.Contracts.Requests;
using RaizesNordesteApi.Contracts.Responses;
using RaizesNordesteApi.Extensions;
using RaizesNordesteApi.Handlers;

namespace RaizesNordesteApi.Controllers;

[ApiController]
[Route("api/unidade")]
[Produces("application/json")]
[Authorize(Roles = "Admin,Gerente")]
public class UnidadeController(
    CriarUnidadeUseCase criarUnidadeUseCase,
    AtualizarUnidadeUseCase atualizarUnidadeUseCase,
    DesativarUnidadeUseCase desativarUnidadeUseCase,
    ListarUnidadesUseCase listarUnidadesUseCase,
    ObterUnidadePorIdUseCase obterUnidadePorIdUseCase,
    ResultadoHttpHandler resultadoHttpHandler)
    : ControllerBase
{
    /// <summary>
    /// Cria uma nova unidade, permitindo que o sistema expanda sua presença física. A unidade é criada com as informações fornecidas, como nome, endereço e status de atividade. Após a criação, a unidade pode ser gerenciada e associada a cardápios e produtos conforme necessário.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A unidade criada com sucesso.</returns>
    [HttpPost]
    public async Task<ActionResult<UnidadeResponse>> CriarUnidade(
        [FromBody] CriarUnidadeRequest request, CancellationToken cancellationToken)
    {
        var response = await criarUnidadeUseCase.ExecutarAsync(request, cancellationToken);
        return resultadoHttpHandler.Tratar(this, ResultadoOperacao<UnidadeResponse>.Criado(response), nameof(ObterUnidadePorId), new { id = response.Id });
    }

    /// <summary>
    /// Lista as unidades, com a opção de filtrar apenas as unidades ativas. Os resultados são paginados para facilitar a navegação e visualização das unidades disponíveis. Essa funcionalidade é útil para exibir as unidades ativas para os clientes ou para fins administrativos, permitindo uma gestão eficiente das unidades cadastradas no sistema.
    /// </summary>
    /// <param name="apenasAtivas"></param>
    /// <param name="paginacao"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Uma resposta paginada contendo as unidades listadas.</returns>
    [HttpGet]
    public async Task<ActionResult<RespostaPaginada<UnidadeResponse>>> ListarUnidade(
        [FromQuery] bool? apenasAtivas, [FromQuery] ConsultaPaginacao paginacao, CancellationToken cancellationToken)
    {
        var response = await listarUnidadesUseCase.ExecutarAsync(apenasAtivas, cancellationToken);
        return resultadoHttpHandler.Tratar(this, ResultadoOperacao<RespostaPaginada<UnidadeResponse>>.Sucesso(response.ParaRespostaPaginada(paginacao)));
    }

    /// <summary>
    /// Obtém os detalhes de uma unidade específica por seu ID, permitindo visualizar informações como nome, endereço e status de atividade. Se a unidade não for encontrada, uma resposta de erro apropriada é retornada. Essa funcionalidade é essencial para acessar informações detalhadas sobre uma unidade específica, seja para fins administrativos ou para exibir detalhes aos clientes.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A unidade correspondente ao ID informado.</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UnidadeResponse>> ObterUnidadePorId(
        [FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await obterUnidadePorIdUseCase.ExecutarAsync(id, cancellationToken);

        var resultado = response is null
            ? ResultadoOperacao<UnidadeResponse>.NaoEncontrado($"Unidade ID {id} não encontrada.")
            : ResultadoOperacao<UnidadeResponse>.Sucesso(response);

        return resultadoHttpHandler.Tratar(this, resultado);
    }

    /// <summary>
    /// Atualiza as informações de uma unidade existente, permitindo modificar detalhes como nome, endereço ou status de atividade. A unidade é identificada por seu ID e as novas informações são fornecidas no corpo da requisição. Se a unidade não for encontrada, uma resposta de erro apropriada é retornada. Essa funcionalidade é crucial para manter as informações das unidades atualizadas e refletir mudanças que possam ocorrer ao longo do tempo.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A unidade atualizada com as novas informações.</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UnidadeResponse>> AtualizarUnidade(
        [FromRoute] int id, [FromBody] AtualizarUnidadeRequest request, CancellationToken cancellationToken)
    {
        var response = await atualizarUnidadeUseCase.ExecutarAsync(id, request, cancellationToken);
        return resultadoHttpHandler.Tratar(this, ResultadoOperacao<UnidadeResponse>.Sucesso(response));
    }

    /// <summary>
    /// Desativa uma unidade específica, tornando-a inativa e indisponível para os clientes. A unidade é identificada por seu ID e, após a desativação, não aparecerá mais nas listagens de unidades ativas. Se a unidade não for encontrada, uma resposta de erro apropriada é retornada. Essa funcionalidade é importante para gerenciar a disponibilidade das unidades e refletir mudanças operacionais, como fechamentos temporários ou permanentes.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Sem conteúdo em caso de sucesso.</returns>
    [HttpPost("{id:int}/desativar")]
    public async Task<IActionResult> DesativarUnidade(
        [FromRoute] int id, CancellationToken cancellationToken)
    {
        await desativarUnidadeUseCase.ExecutarAsync(id, cancellationToken);
        return resultadoHttpHandler.Tratar(this, ResultadoOperacao.SemConteudo());
    }
}
