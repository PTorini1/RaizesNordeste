using RaizesNordeste.Domain.Enums;

namespace RaizesNordeste.Application.DTOs.Usuarios;

public class CriarUsuarioRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public RoleUsuario Role { get; set; } = RoleUsuario.Cliente;
    public bool Ativo { get; set; } = true;
}
