using RaizesNordeste.Domain.Interfaces.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace RaizesNordeste.Infrastructure.Cache;

public class RedisCacheService(IConnectionMultiplexer connectionMultiplexer) : IRedisCacheService
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default) where T : class
    {
        cancellationToken.ThrowIfCancellationRequested();
        var json = JsonSerializer.Serialize(value, SerializerOptions);
        await _database.StringSetAsync(key, json, expiry, When.Always);
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        cancellationToken.ThrowIfCancellationRequested();
        var value = await _database.StringGetAsync(key);
        if (!value.HasValue)
        {
            return null;
        }

        return JsonSerializer.Deserialize<T>(value.ToString(), SerializerOptions);
    }

    public async Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _database.KeyDeleteAsync(key);
    }

    public async Task<bool> SetStringWithLockAsync(string key, string value, TimeSpan expiry, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _database.StringSetAsync(key, value, expiry, When.NotExists);
    }
}
