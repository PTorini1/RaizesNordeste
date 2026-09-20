using RaizesNordeste.Domain.Enums;

namespace RaizesNordeste.Domain.Interfaces.Services;

public interface IFirebaseAuthService
{
    Task<(bool Sucesso, string Email, string TokenHash, RoleUsuario Role)> ValidarTokenFirebaseAsync(string token, CancellationToken cancellationToken = default);
    Task<string> CriarUsuarioFirebaseAsync(string email, string senha, CancellationToken cancellationToken = default);
    Task<(bool Sucesso, string Token, string Email, string Uid, string MensagemErro)> AutenticarComEmailSenhaAsync(string email, string senha, CancellationToken cancellationToken = default);
}
