using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Interfaces.Services;
using RaizesNordeste.Infrastructure.Firebase.Models;
using System.Net.Http.Json;
using System.Security.Claims;

namespace RaizesNordeste.Infrastructure.Firebase;

public class FirebaseAuthService(IConfiguration configuration) : IFirebaseAuthService
{
    private readonly IConfiguration _configuration = configuration;
    private static readonly HttpClient _httpClient = new();
    private static ConfigurationManager<OpenIdConnectConfiguration>? _configManager;
    private static readonly object _lock = new();

    private static ConfigurationManager<OpenIdConnectConfiguration> GetConfigurationManager(string issuer)
    {
        if (_configManager is not null)
        {
            return _configManager;
        }

        lock (_lock)
        {
            _configManager ??= new ConfigurationManager<OpenIdConnectConfiguration>(
                $"{issuer}/.well-known/openid-configuration",
                new OpenIdConnectConfigurationRetriever(),
                new HttpDocumentRetriever());
        }

        return _configManager;
    }

    public async Task<(bool Sucesso, string Email, string TokenHash, RoleUsuario Role)> ValidarTokenFirebaseAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return (false, string.Empty, string.Empty, RoleUsuario.Cliente);
        }

        try
        {
            var handler = new JsonWebTokenHandler();
            if (!handler.CanReadToken(token))
            {
                return (false, string.Empty, string.Empty, RoleUsuario.Cliente);
            }

            var projectId = _configuration["Firebase:ProjectId"] ?? "raizes-nordeste";
            var issuer = $"https://securetoken.google.com/{projectId}";
            var configManager = GetConfigurationManager(issuer);
            var openIdConfig = await configManager.GetConfigurationAsync(cancellationToken);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = projectId,
                ValidateLifetime = true,
                IssuerSigningKeys = openIdConfig.SigningKeys
            };

            var validationResult = await handler.ValidateTokenAsync(token, validationParameters);
            if (!validationResult.IsValid)
            {
                return (false, string.Empty, string.Empty, RoleUsuario.Cliente);
            }

            var email = validationResult.ClaimsIdentity.FindFirst(ClaimTypes.Email)?.Value
                ?? validationResult.ClaimsIdentity.FindFirst("email")?.Value
                ?? string.Empty;

            var uid = validationResult.ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? validationResult.ClaimsIdentity.FindFirst("sub")?.Value
                ?? string.Empty;

            return string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(uid)
                ? (false, string.Empty, string.Empty, RoleUsuario.Cliente)
                : (true, email, uid, RoleUsuario.Cliente);
        }
        catch
        {
            return (false, string.Empty, string.Empty, RoleUsuario.Cliente);
        }
    }

    public async Task<string> CriarUsuarioFirebaseAsync(string email, string senha, CancellationToken cancellationToken = default)
    {
        var apiKey = _configuration["Firebase:ApiKey"] ?? string.Empty;
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Firebase:ApiKey não configurada.");
        }

        var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={apiKey}";
        var payload = new
        {
            email,
            password = senha,
            returnSecureToken = true
        };

        var response = await _httpClient.PostAsJsonAsync(url, payload, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException($"Erro ao cadastrar usuário no Firebase: {errorContent}");
        }

        var result = await response.Content.ReadFromJsonAsync<FirebaseSignUpResponse>(cancellationToken: cancellationToken);
        if (result is null || string.IsNullOrWhiteSpace(result.LocalId))
        {
            throw new InvalidOperationException("Resposta do Firebase inválida ao criar usuário.");
        }

        return result.LocalId;
    }

    public async Task<(bool Sucesso, string Token, string Email, string Uid, string MensagemErro)> AutenticarComEmailSenhaAsync(
        string email,
        string senha,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
        {
            return (false, string.Empty, string.Empty, string.Empty, "E-mail e senha são obrigatórios.");
        }

        try
        {
            var apiKey = _configuration["Firebase:ApiKey"] ?? string.Empty;
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return (false, string.Empty, string.Empty, string.Empty, "Firebase:ApiKey não configurada.");
            }

            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={apiKey}";
            var payload = new
            {
                email,
                password = senha,
                returnSecureToken = true
            };

            var response = await _httpClient.PostAsJsonAsync(url, payload, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadFromJsonAsync<FirebaseErrorResponse>(cancellationToken: cancellationToken);
                var errorMsg = errorContent?.Error?.Message ?? "Erro ao autenticar no Firebase.";

                errorMsg = errorMsg switch
                {
                    "EMAIL_NOT_FOUND" => "E-mail não cadastrado.",
                    "INVALID_PASSWORD" => "Senha incorreta.",
                    "USER_DISABLED" => "Este usuário foi desativado.",
                    _ => errorMsg
                };

                return (false, string.Empty, string.Empty, string.Empty, errorMsg);
            }

            var successContent = await response.Content.ReadFromJsonAsync<FirebaseSignInResponse>(cancellationToken: cancellationToken);
            if (successContent is null || string.IsNullOrWhiteSpace(successContent.IdToken))
            {
                return (false, string.Empty, string.Empty, string.Empty, "Resposta do Firebase inválida.");
            }

            return (true, successContent.IdToken, successContent.Email, successContent.LocalId, string.Empty);
        }
        catch (Exception ex)
        {
            return (false, string.Empty, string.Empty, string.Empty, $"Erro de conexão com o Firebase: {ex.Message}");
        }
    }
}
