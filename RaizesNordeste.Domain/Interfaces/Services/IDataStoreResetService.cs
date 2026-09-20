namespace RaizesNordeste.Domain.Interfaces.Services;

public interface IDataStoreResetService
{
    Task ResetAsync(CancellationToken cancellationToken = default);
}
