using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesNordeste.Application.Common;
using RaizesNordeste.Application.DTOs.Promocoes;
using RaizesNordeste.Application.UseCases.Promocoes;
using RaizesNordeste.Domain.Enums;
using RaizesNordesteApi.Contracts.Requests;
using RaizesNordesteApi.Contracts.Responses;
using RaizesNordesteApi.Extensions;
using RaizesNordesteApi.Handlers;

namespace RaizesNordesteApi.Controllers;

[ApiController]
[Route("api/promocao")]
[Produces("application/json")]
[Authorize]
public class PromocaoController(
    CriarPromocaoUseCase criarPromocaoUseCase,
    ListarPromocoesUseCase listarPromocoesUseCase,
    ListarPromocoesAtivasUseCase listarPromocoesAtivasUseCase,
    ResultadoHttpHandler resultadoHttpHandler)
    : ControllerBase
{
    /// <summary>
    /// Cria uma nova promoção com base nas informações fornecidas. A promoção é adicionada ao sistema e pode ser aplicada a produtos ou categorias específicas, dependendo dos critérios definidos. Essa operação é restrita a usuários com as funções de Admin ou Gerente, garantindo que apenas usuários autorizados possam criar promoções e gerenciar as ofertas disponíveis para os clientes.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A promoção criada.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin,Gerente")]
    public async Task<ActionResult<PromocaoResponse>> CriarPromocao(
        [FromBody] CriarPromocaoRequest request, CancellationToken cancellationToken)
    {
        var response = await criarPromocaoUseCase.ExecutarAsync(request, cancellationToken);
        return resultadoHttpHandler.Tratar(this, ResultadoOperacao<PromocaoResponse>.Criado(response));
    }

    /// <summary>
    /// Lista as promoções existentes no sistema, permitindo que os usuários visualizem as ofertas disponíveis. Os resultados são paginados para facilitar a navegação e a visualização das promoções, especialmente quando há um grande número de ofertas ativas. Essa operação é restrita a usuários com as funções de Admin ou Gerente, garantindo que apenas usuários autorizados possam acessar a lista completa de promoções e gerenciar as ofertas disponíveis para os clientes.
    /// </summary>
    /// <param name="paginacao"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Uma resposta paginada contendo a lista de promoções.</returns>
    [HttpGet]
    [Authorize(Roles = "Admin,Gerente")]
    public async Task<ActionResult<RespostaPaginada<PromocaoResponse>>> ListarPromocao(
        [FromQuery] ConsultaPaginacao paginacao, CancellationToken cancellationToken)
    {
        var response = await listarPromocoesUseCase.ExecutarAsync(cancellationToken);
        return resultadoHttpHandler.Tratar(this, ResultadoOperacao<RespostaPaginada<PromocaoResponse>>.Sucesso(response.ParaRespostaPaginada(paginacao)));
    }

    /// <summary>
    /// Lista as promoções ativas, ou seja, aquelas que estão atualmente em vigor e podem ser aplicadas a produtos ou categorias específicas. Os resultados podem ser filtrados por critérios como produto, canal de pedido ou perfil do cliente, permitindo que os usuários visualizem apenas as promoções relevantes para suas necessidades. Os resultados são paginados para facilitar a navegação e a visualização das promoções ativas, especialmente quando há um grande número de ofertas disponíveis. Essa operação é acessível a usuários com as funções de Admin, Gerente ou Cliente, garantindo que todos os tipos de usuários possam visualizar as promoções ativas e aproveitar as ofertas disponíveis para eles.
    /// </summary>
    /// <param name="produtoId"></param>
    /// <param name="canalPedido"></param>
    /// <param name="perfilCliente"></param>
    /// <param name="paginacao"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Uma resposta paginada contendo a lista de promoções ativas.</returns>
    [HttpGet("ativa")]
    [Authorize(Roles = "Admin,Gerente,Cliente")]
    public async Task<ActionResult<RespostaPaginada<PromocaoResponse>>> ListarPromocaoAtivas(
        [FromQuery] int? produtoId, [FromQuery] CanalPedido? canalPedido, [FromQuery] string? perfilCliente,
        [FromQuery] ConsultaPaginacao paginacao, CancellationToken cancellationToken)
    {
        var response = await listarPromocoesAtivasUseCase.ExecutarAsync(
            produtoId,
            canalPedido,
            perfilCliente,
            cancellationToken);

        return resultadoHttpHandler.Tratar(this, ResultadoOperacao<RespostaPaginada<PromocaoResponse>>.Sucesso(response.ParaRespostaPaginada(paginacao)));
    }
}
