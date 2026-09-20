namespace RaizesNordeste.Domain.Interfaces.Services;

public interface IRedisCacheService
{
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default) where T : class;
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;
    Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default);
    Task<bool> SetStringWithLockAsync(string key, string value, TimeSpan expiry, CancellationToken cancellationToken = default);
}
