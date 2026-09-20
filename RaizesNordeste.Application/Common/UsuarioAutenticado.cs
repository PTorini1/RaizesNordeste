namespace RaizesNordeste.Application.Common;

public sealed record UsuarioAutenticado(
    int? UsuarioId,
    IReadOnlyCollection<string> Roles)
{
    public bool EhCliente => Roles.Contains("Cliente", StringComparer.OrdinalIgnoreCase);
}
