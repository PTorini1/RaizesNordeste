namespace RaizesNordeste.Domain.Interfaces.Services;

public interface IDataStoreInitializerService
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
}
