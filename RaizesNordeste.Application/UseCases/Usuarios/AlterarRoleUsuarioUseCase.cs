using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Services;

namespace RaizesNordeste.Application.UseCases.Usuarios;

public class AlterarRoleUsuarioUseCase(
    IUsuarioRepository usuarioRepository,
    IAuditoriaRepository auditoriaRepository,
    UsuarioDomainService usuarioDomainService)
{
    public async Task<Usuario?> ExecutarAsync(int id, RoleUsuario novaRole, CancellationToken cancellationToken = default)
    {
        var usuario = await usuarioRepository.GetByIdAsync(id, cancellationToken);
        if (usuario is null)
        {
            return null;
        }

        var roleAnterior = usuario.Role;
        if (roleAnterior == novaRole)
        {
            return usuario;
        }

        usuarioDomainService.AlterarRole(usuario, novaRole);
        await usuarioRepository.UpdateAsync(usuario, cancellationToken);

        var auditoria = new Auditoria
        {
            UsuarioId = id,
            Entidade = "Usuario",
            EntidadeId = usuario.Id,
            Acao = "ALTERAR_PERMISSOES",
            DadosAnteriores = $"Role anterior: {roleAnterior}",
            DadosNovos = $"Role nova: {novaRole}",
            CriadoEm = DateTime.UtcNow
        };
        await auditoriaRepository.InsertAsync(auditoria, cancellationToken);

        return usuario;
    }
}
