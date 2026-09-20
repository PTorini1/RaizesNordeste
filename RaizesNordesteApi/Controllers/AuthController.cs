using Microsoft.AspNetCore.Mvc;
using RaizesNordeste.Application.Common;
using RaizesNordeste.Application.UseCases.Autenticacao;
using RaizesNordeste.Domain.Entities;
using RaizesNordesteApi.Contracts.Requests;
using RaizesNordesteApi.Contracts.Responses;
using RaizesNordesteApi.Handlers;

namespace RaizesNordesteApi.Controllers;

[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public class AuthController(
    AutenticarUsuarioUseCase autenticarUsuarioUseCase,
    AutenticarUsuarioEmailSenhaUseCase autenticarUsuarioEmailSenhaUseCase,
    ResultadoHttpHandler resultadoHttpHandler)
    : ControllerBase
{
    /// <summary>
    /// Autentica um usuário utilizando um token do Firebase. O token é validado e, se for válido, o usuário correspondente é retornado. Caso o token seja inválido ou o usuário esteja inativo, uma resposta de erro apropriada é retornada.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>O usuário autenticado correspondente ao token do Firebase fornecido.</returns>
    [HttpPost("login")]
    public async Task<ActionResult<Usuario>> Login(
        [FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
        {
            return resultadoHttpHandler.Tratar(
                this,
                ResultadoOperacao<Usuario>.BadRequest("TOKEN_OBRIGATORIO", "O token do Firebase é obrigatório."));
        }

        try
        {
            var usuario = await autenticarUsuarioUseCase.ExecutarAsync(request.Token, cancellationToken);
            return resultadoHttpHandler.Tratar(this, ResultadoOperacao<Usuario>.Sucesso(usuario));
        }
        catch (ArgumentException ex)
        {
            return resultadoHttpHandler.Tratar(
                this,
                ResultadoOperacao<Usuario>.BadRequest("TOKEN_INVALIDO", ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return resultadoHttpHandler.Tratar(
                this,
                ResultadoOperacao<Usuario>.Validacao(ex.Message, codigo: "USUARIO_INATIVO"));
        }
    }

    /// <summary>
    /// Autentica um usuário utilizando email e senha. As credenciais são validadas e, se forem válidas, o usuário correspondente e um token do Firebase são retornados. Caso as credenciais sejam inválidas ou o usuário esteja inativo, uma resposta de erro apropriada é retornada.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>As credenciais de login contendo o usuário autenticado e o token do Firebase.</returns>
    [HttpPost("login-credentials")]
    public async Task<ActionResult<LoginCredentialsResponse>> LoginCredentials(
        [FromBody] LoginCredentialsRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
        {
            return resultadoHttpHandler.Tratar(
                this,
                ResultadoOperacao<LoginCredentialsResponse>.BadRequest(
                    "CREDENCIAIS_OBRIGATORIAS",
                    "E-mail e senha são obrigatórios."));
        }

        try
        {
            var (usuario, tokenFirebase) = await autenticarUsuarioEmailSenhaUseCase.ExecutarAsync(request.Email, request.Senha, cancellationToken);

            return resultadoHttpHandler.Tratar(
                this,
                ResultadoOperacao<LoginCredentialsResponse>.Sucesso(new LoginCredentialsResponse
                {
                    Usuario = usuario,
                    Token = tokenFirebase
                }));
        }
        catch (ArgumentException ex)
        {
            return resultadoHttpHandler.Tratar(
                this,
                ResultadoOperacao<LoginCredentialsResponse>.BadRequest("LOGIN_FALHOU", ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return resultadoHttpHandler.Tratar(
                this,
                ResultadoOperacao<LoginCredentialsResponse>.Validacao(ex.Message, codigo: "USUARIO_INATIVO"));
        }
    }
}
