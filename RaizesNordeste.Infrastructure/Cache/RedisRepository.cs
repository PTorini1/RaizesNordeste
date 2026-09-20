using RaizesNordeste.Domain.Interfaces.Repositories;
using StackExchange.Redis;
using System.Text.Json;

namespace RaizesNordeste.Infrastructure.Cache;

public class RedisRepository<TEntity>(IConnectionMultiplexer connectionMultiplexer)
    : IRedisRepository<TEntity>
    where TEntity : class
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

    public async Task<TEntity> InsertAsync<TKey>(
        TKey id,
        TEntity entity,
        TimeSpan? expiry = null,
        CancellationToken cancellationToken = default)
        where TKey : notnull
    {
        cancellationToken.ThrowIfCancellationRequested();

        await SetAsync(id, entity, expiry);

        return entity;
    }

    public async Task<TEntity?> GetByIdAsync<TKey>(
        TKey id,
        CancellationToken cancellationToken = default)
        where TKey : notnull
    {
        cancellationToken.ThrowIfCancellationRequested();

        var value = await _database.StringGetAsync(BuildKey(id));

        return value.HasValue
            ? JsonSerializer.Deserialize<TEntity>((string)value!, SerializerOptions)
            : null;
    }

    public async Task<IReadOnlyList<TEntity>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var endpoints = connectionMultiplexer.GetEndPoints();
        var entities = new List<TEntity>();

        foreach (var endpoint in endpoints)
        {
            var server = connectionMultiplexer.GetServer(endpoint);

            await foreach (var key in server.KeysAsync(pattern: $"{BuildPrefix()}*"))
            {
                cancellationToken.ThrowIfCancellationRequested();

                var value = await _database.StringGetAsync(key);

                if (!value.HasValue)
                {
                    continue;
                }

                var entity = JsonSerializer.Deserialize<TEntity>((string)value!, SerializerOptions);

                if (entity is not null)
                {
                    entities.Add(entity);
                }
            }
        }

        return entities;
    }

    public async Task<TEntity> UpdateAsync<TKey>(
        TKey id,
        TEntity entity,
        TimeSpan? expiry = null,
        CancellationToken cancellationToken = default)
        where TKey : notnull
    {
        cancellationToken.ThrowIfCancellationRequested();

        await SetAsync(id, entity, expiry);

        return entity;
    }

    public async Task<TEntity?> PatchAsync<TKey>(
        TKey id,
        Action<TEntity> patch,
        TimeSpan? expiry = null,
        CancellationToken cancellationToken = default)
        where TKey : notnull
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await GetByIdAsync(id, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        patch(entity);
        await SetAsync(id, entity, expiry);

        return entity;
    }

    public async Task<bool> DeleteByIdAsync<TKey>(
        TKey id,
        CancellationToken cancellationToken = default)
        where TKey : notnull
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _database.KeyDeleteAsync(BuildKey(id));
    }

    private async Task SetAsync<TKey>(
        TKey id,
        TEntity entity,
        TimeSpan? expiry)
        where TKey : notnull
    {
        var value = JsonSerializer.Serialize(entity, SerializerOptions);

        await _database.StringSetAsync(BuildKey(id), value, expiry, When.Always);
    }

    private static string BuildKey<TKey>(TKey id)
        where TKey : notnull
    {
        return $"{BuildPrefix()}{id}";
    }

    private static string BuildPrefix()
    {
        return $"{typeof(TEntity).Name}:";
    }
}
