using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases.Usuarios;

public class ListarUsuariosUseCase(IUsuarioRepository usuarioRepository)
{
    public async Task<IReadOnlyList<Usuario>> ExecutarAsync(CancellationToken cancellationToken = default)
    {
        return await usuarioRepository.ListAsync(cancellationToken: cancellationToken);
    }
}
