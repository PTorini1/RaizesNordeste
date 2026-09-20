using RaizesNordeste.Application.Common;
using System.Security.Claims;

namespace RaizesNordesteApi.Extensions;

public static class UsuarioAutenticadoExtensions
{
    public static UsuarioAutenticado ObterUsuarioAutenticado(this ClaimsPrincipal user)
    {
        var usuarioIdClaim = user.FindFirst("usuario_id")?.Value;
        int? usuarioId = int.TryParse(usuarioIdClaim, out var id) ? id : null;

        var roles = user.FindAll(ClaimTypes.Role)
            .Select(claim => claim.Value)
            .Concat(user.FindAll("role").Select(claim => claim.Value))
            .Concat(user.FindAll("roles").Select(claim => claim.Value))
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return new UsuarioAutenticado(usuarioId, roles);
    }
}
