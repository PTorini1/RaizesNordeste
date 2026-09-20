using RaizesNordeste.Domain.Enums;

namespace RaizesNordeste.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirebaseUid { get; set; } = string.Empty;
    public RoleUsuario Role { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public RoleUsuario AlterarRole(RoleUsuario novaRole)
    {
        var roleAnterior = Role;
        Role = novaRole;
        return roleAnterior;
    }
}
