using RaizesNordeste.Application.DTOs.Usuarios;
using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Interfaces.Services;
using RaizesNordeste.Domain.Services;

namespace RaizesNordeste.Application.UseCases.Usuarios;

public class CriarUsuarioUseCase(
    IUsuarioRepository usuarioRepository,
    IFirebaseAuthService firebaseAuthService,
    UsuarioDomainService usuarioDomainService)
{
    public async Task<Usuario> ExecutarAsync(CriarUsuarioRequest request, CancellationToken cancellationToken = default)
    {
        var usuarioParaValidacao = usuarioDomainService.CriarUsuario(request.Nome, request.Email, "firebase-pendente", request.Role, request.Ativo);
        _ = usuarioParaValidacao;

        var existentes = await usuarioRepository.ListAsync(u => u.Email == request.Email, cancellationToken: cancellationToken);
        usuarioDomainService.ValidarEmailDisponivel(existentes, request.Email);

        var firebaseUid = await firebaseAuthService.CriarUsuarioFirebaseAsync(
            request.Email,
            request.Senha,
            cancellationToken);

        var usuario = usuarioDomainService.CriarUsuario(request.Nome, request.Email, firebaseUid, request.Role, request.Ativo);
        return await usuarioRepository.InsertAsync(usuario, cancellationToken);
    }
}
