using Applications.Usecase.Common.Models;
using SharedKernel;
using System.Linq.Expressions;

namespace Applications.Usecase.Common.Interfaces;

public interface IGenericRepository<TEntity, TID> : IDisposable
    where TEntity : Entity<TID>
{
    Task<TEntity> GetAsync(TID id, FindOptions<TEntity>? findOptions = null, CancellationToken token = default);
    Task<List<TEntity>> GetAllAsync(FindOptions<TEntity>? findOptions = null, CancellationToken token = default);
    Task<PaginatedListDTO<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>> predicate
        , string orderBy
        , int page
        , int size
        , FindOptions<TEntity>? findOptions = null
        , CancellationToken token = default
        );
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token);
    Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);
    Task<IEnumerable<string>> NavigationsAsync(CancellationToken token = default);
    Task<TID> InsertAsync(TEntity entity);
    Task<bool> InsertRangeAsync(IEnumerable<TEntity> entities);
    Task<TID> UpdateAsync(TEntity entity);
    Task<bool> UpdateBatchAsync(IEnumerable<TEntity> entities);
    Task<TID> DeleteAsync(TID id);
    Task<bool> DeleteRangeAsync(IEnumerable<TEntity> entities);
}
public class FindOptions<T>
{
    public bool IsIgnoreAutoIncludes { get; set; }
    public bool IsAsNoTracking { get; set; }
    public Expression<Func<T, object>>[] Includes { get; set; } = [];

    public static FindOptions<T> ReportOptions(bool ignoreAutoIncludes = true) =>
        new FindOptions<T> { IsIgnoreAutoIncludes = ignoreAutoIncludes, IsAsNoTracking = true };
    public static FindOptions<T> SetOptions(bool ignoreAutoIncludes = true) =>
        new FindOptions<T> { IsIgnoreAutoIncludes = ignoreAutoIncludes, IsAsNoTracking = false };
}