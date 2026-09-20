namespace RaizesNordeste.Infrastructure.Firebase.Models;

internal class FirebaseSignInResponse
{
    public string IdToken { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
}
