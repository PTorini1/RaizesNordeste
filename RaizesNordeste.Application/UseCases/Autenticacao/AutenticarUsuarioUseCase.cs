using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Interfaces.Services;
using RaizesNordeste.Domain.Services;

namespace RaizesNordeste.Application.UseCases.Autenticacao;

public class AutenticarUsuarioUseCase(
    IFirebaseAuthService firebaseAuthService,
    IUsuarioRepository usuarioRepository,
    IClienteRepository clienteRepository,
    UsuarioDomainService usuarioDomainService)
{
    public async Task<Usuario> ExecutarAsync(string token, CancellationToken cancellationToken = default)
    {
        var (sucesso, email, tokenHash, _) = await firebaseAuthService.ValidarTokenFirebaseAsync(token, cancellationToken);
        if (!sucesso)
        {
            throw new ArgumentException("Token do Firebase inválido ou expirado.");
        }

        var usuarios = await usuarioRepository.ListAsync(u => u.Email == email, cancellationToken: cancellationToken);
        var usuario = usuarios.FirstOrDefault();

        if (usuario is null)
        {
            usuario = usuarioDomainService.ResolverAutoRegistro(email, tokenHash);
            usuario = await usuarioRepository.InsertAsync(usuario, cancellationToken);

            var cliente = usuarioDomainService.CriarClienteParaUsuario(usuario.Id);
            await clienteRepository.InsertAsync(cliente, cancellationToken);
        }
        else
        {
            usuarioDomainService.ValidarAtivo(usuario);
        }

        return usuario;
    }
}
