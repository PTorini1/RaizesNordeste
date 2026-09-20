namespace RaizesNordeste.Infrastructure.Firebase.Models;

internal class FirebaseErrorResponse
{
    public FirebaseErrorDetails? Error { get; set; }
}

internal class FirebaseErrorDetails
{
    public string Message { get; set; } = string.Empty;
}
