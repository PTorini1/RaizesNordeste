namespace RaizesNordeste.Domain.Interfaces.Repositories;

public interface IRedisRepository<TEntity>
    where TEntity : class
{
    Task<TEntity> InsertAsync<TKey>(
        TKey id,
        TEntity entity,
        TimeSpan? expiry = null,
        CancellationToken cancellationToken = default)
        where TKey : notnull;

    Task<TEntity?> GetByIdAsync<TKey>(
        TKey id,
        CancellationToken cancellationToken = default)
        where TKey : notnull;

    Task<IReadOnlyList<TEntity>> ListAsync(
        CancellationToken cancellationToken = default);

    Task<TEntity> UpdateAsync<TKey>(
        TKey id,
        TEntity entity,
        TimeSpan? expiry = null,
        CancellationToken cancellationToken = default)
        where TKey : notnull;

    Task<TEntity?> PatchAsync<TKey>(
        TKey id,
        Action<TEntity> patch,
        TimeSpan? expiry = null,
        CancellationToken cancellationToken = default)
        where TKey : notnull;

    Task<bool> DeleteByIdAsync<TKey>(
        TKey id,
        CancellationToken cancellationToken = default)
        where TKey : notnull;
}
