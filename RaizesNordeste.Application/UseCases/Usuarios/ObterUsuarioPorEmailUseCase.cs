using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases.Usuarios;

public class ObterUsuarioPorEmailUseCase(IUsuarioRepository usuarioRepository)
{
    public async Task<Usuario?> ExecutarAsync(string email, CancellationToken cancellationToken = default)
    {
        var usuarios = await usuarioRepository.ListAsync(u => u.Email == email, cancellationToken: cancellationToken);
        return usuarios.FirstOrDefault();
    }
}
