using RaizesNordeste.Domain.Entities;

namespace RaizesNordesteApi.Contracts.Responses;

public class LoginCredentialsResponse
{
    public Usuario Usuario { get; set; } = null!;
    public string Token { get; set; } = string.Empty;
}
