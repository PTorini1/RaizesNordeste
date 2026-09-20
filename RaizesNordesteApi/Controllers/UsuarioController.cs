using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesNordeste.Application.Common;
using RaizesNordeste.Application.DTOs.Usuarios;
using RaizesNordeste.Application.UseCases.Usuarios;
using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Enums;
using RaizesNordesteApi.Contracts.Requests;
using RaizesNordesteApi.Contracts.Responses;
using RaizesNordesteApi.Extensions;
using RaizesNordesteApi.Handlers;

namespace RaizesNordesteApi.Controllers;

[ApiController]
[Route("api/usuario")]
[Produces("application/json")]
[Authorize(Roles = "Admin")]
public class UsuarioController(
    CriarUsuarioUseCase criarUsuarioUseCase,
    ListarUsuariosUseCase listarUsuariosUseCase,
    AlterarRoleUsuarioUseCase alterarRoleUsuarioUseCase,
    ResultadoHttpHandler resultadoHttpHandler)
    : ControllerBase
{
    /// <summary>
    /// Cria um novo usuário no sistema, utilizando as informações fornecidas na requisição. O endpoint recebe os dados necessários para criar o usuário, como nome, email e role, e retorna o usuário criado com um status HTTP 201 Created. Caso haja algum erro de validação ou se o email já estiver em uso, uma resposta de erro apropriada é retornada.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>O usuário criado.</returns>
    [HttpPost]
    public async Task<ActionResult<Usuario>> CriarUsuario(
        [FromBody] CriarUsuarioRequest request, CancellationToken cancellationToken)
    {
        var salvo = await criarUsuarioUseCase.ExecutarAsync(request, cancellationToken);
        return resultadoHttpHandler.Tratar(this, ResultadoOperacao<Usuario>.Criado(salvo));
    }

    /// <summary>
    /// Lista os usuários do sistema, permitindo a paginação dos resultados. O endpoint retorna uma lista paginada de usuários, contendo informações como nome, email e role. Os resultados podem ser filtrados e ordenados conforme necessário, e a resposta inclui metadados de paginação para facilitar a navegação pelos resultados.
    /// </summary>
    /// <param name="paginacao"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Uma resposta paginada contendo a lista de usuários.</returns>
    [HttpGet]
    public async Task<ActionResult<RespostaPaginada<Usuario>>> ListarUsuario(
        [FromQuery] ConsultaPaginacao paginacao, CancellationToken cancellationToken)
    {
        var lista = await listarUsuariosUseCase.ExecutarAsync(cancellationToken);
        return resultadoHttpHandler.Tratar(this, ResultadoOperacao<RespostaPaginada<Usuario>>.Sucesso(lista.ParaRespostaPaginada(paginacao)));
    }

    /// <summary>
    /// Altera a role de um usuário específico, permitindo modificar as permissões e acessos do usuário no sistema. O endpoint recebe o ID do usuário e a nova role a ser atribuída, e retorna o usuário atualizado with a nova role. Caso o usuário não seja encontrado ou se a nova role for inválida, uma resposta de erro apropriada é retornada.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="novaRole"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>O usuário atualizado com a nova role.</returns>
    [HttpPut("{id:int}/role")]
    public async Task<ActionResult<Usuario>> AlterarRole(
        [FromRoute] int id, [FromBody] RoleUsuario novaRole, CancellationToken cancellationToken)
    {
        var usuario = await alterarRoleUsuarioUseCase.ExecutarAsync(id, novaRole, cancellationToken);

        var resultado = usuario is null
            ? ResultadoOperacao<Usuario>.NaoEncontrado($"Usuário ID {id} não encontrado.")
            : ResultadoOperacao<Usuario>.Sucesso(usuario);

        return resultadoHttpHandler.Tratar(this, resultado);
    }
}
