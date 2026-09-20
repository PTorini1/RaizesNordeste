using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Interfaces.Services;
using RaizesNordeste.Domain.Services;

namespace RaizesNordeste.Application.UseCases.Autenticacao;

public class AutenticarUsuarioEmailSenhaUseCase(
    IFirebaseAuthService firebaseAuthService,
    IUsuarioRepository usuarioRepository,
    IClienteRepository clienteRepository,
    UsuarioDomainService usuarioDomainService)
{
    public async Task<(Usuario Usuario, string Token)> ExecutarAsync(string email, string senha, CancellationToken cancellationToken = default)
    {
        var (sucesso, token, resEmail, uid, erroMsg) = await firebaseAuthService.AutenticarComEmailSenhaAsync(email, senha, cancellationToken);
        if (!sucesso)
        {
            throw new ArgumentException(erroMsg);
        }

        var usuarios = await usuarioRepository.ListAsync(u => u.Email == resEmail, cancellationToken: cancellationToken);
        var usuario = usuarios.FirstOrDefault();

        if (usuario is null)
        {
            usuario = usuarioDomainService.ResolverAutoRegistro(resEmail, uid);
            usuario = await usuarioRepository.InsertAsync(usuario, cancellationToken);

            var cliente = usuarioDomainService.CriarClienteParaUsuario(usuario.Id);
            await clienteRepository.InsertAsync(cliente, cancellationToken);
        }
        else
        {
            usuarioDomainService.ValidarAtivo(usuario);
        }

        return (usuario, token);
    }
}
