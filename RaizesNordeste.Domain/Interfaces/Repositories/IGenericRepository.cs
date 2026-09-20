using System.Linq.Expressions;

namespace RaizesNordeste.Domain.Interfaces.Repositories;

public interface IGenericRepository<TEntity>
    where TEntity : class
{
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default)
        where TKey : notnull;

    Task<IReadOnlyList<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        int? skip = null,
        int? take = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<TEntity?> PatchAsync<TKey>(
        TKey id,
        Action<TEntity> patch,
        CancellationToken cancellationToken = default)
        where TKey : notnull;

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<bool> DeleteByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default)
        where TKey : notnull;
}
