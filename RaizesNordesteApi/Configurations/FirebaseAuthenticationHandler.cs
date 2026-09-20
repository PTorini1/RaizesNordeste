using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using RaizesNordeste.Application.UseCases.Usuarios;
using RaizesNordeste.Domain.Interfaces.Services;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace RaizesNordesteApi.Configurations;

public class FirebaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IFirebaseAuthService _firebaseAuthService;
    private readonly IServiceProvider _serviceProvider;

    public FirebaseAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IFirebaseAuthService firebaseAuthService,
        IServiceProvider serviceProvider)
        : base(options, logger, encoder)
    {
        _firebaseAuthService = firebaseAuthService;
        _serviceProvider = serviceProvider;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authorizationValues))
        {
            return AuthenticateResult.NoResult();
        }

        var authorization = authorizationValues.FirstOrDefault();
        if (string.IsNullOrEmpty(authorization) || !authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.NoResult();
        }

        var token = authorization["Bearer ".Length..].Trim();
        if (string.IsNullOrEmpty(token))
        {
            return AuthenticateResult.Fail("Token vazio.");
        }

        try
        {
            var (sucesso, email, firebaseUid, _) = await _firebaseAuthService.ValidarTokenFirebaseAsync(
                token,
                Context.RequestAborted);

            if (!sucesso)
            {
                return AuthenticateResult.Fail("Token inválido.");
            }

            using var scope = _serviceProvider.CreateScope();
            var obterUsuarioPorEmailUseCase = scope.ServiceProvider.GetRequiredService<ObterUsuarioPorEmailUseCase>();
            var usuario = await obterUsuarioPorEmailUseCase.ExecutarAsync(email, Context.RequestAborted);

            if (usuario is null)
            {
                return AuthenticateResult.Fail("Usuário não cadastrado no banco relacional.");
            }

            if (!usuario.Ativo)
            {
                return AuthenticateResult.Fail("Usuário inativo no sistema.");
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, email),
                new(ClaimTypes.Role, usuario.Role.ToString()),
                new("role", usuario.Role.ToString()),
                new("roles", usuario.Role.ToString()),
                new("firebase_uid", firebaseUid),
                new("usuario_id", usuario.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name, ClaimTypes.Email, ClaimTypes.Role);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch (Exception ex)
        {
            return AuthenticateResult.Fail(ex);
        }
    }
}
