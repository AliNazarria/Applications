using Applications.Usecase.Common.Interfaces;
using Applications.Usecase.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel;
using System.Linq.Expressions;

namespace Applications.Usecase.Common;

public class GenericRepositoryDeletedProxy<TEntity, TID>(
      [FromKeyedServices("real")] IGenericRepository<TEntity, TID> repository
    )
    : IGenericRepository<TEntity, TID>
    where TEntity : Entity<TID>
{
    public async Task<TEntity> GetAsync(TID id, FindOptions<TEntity>? findOptions = null, CancellationToken token = default)
    {
        var result = await repository.GetAsync(id, findOptions, token);
        if (result is null || result.Deleted)
            return null;
        return result;
    }
    public async Task<PaginatedListDTO<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>> predicate, string orderBy, int page, int size, FindOptions<TEntity>? findOptions = null, CancellationToken token = default)
    {
        predicate = predicate.And(x => x.Deleted == false);
        return await repository.GetPagedAsync(predicate, orderBy, page, size, findOptions, token);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
    {
        predicate = predicate.And(x => x.Deleted == false);
        return await repository.CountAsync(predicate, token);
    }

    public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
    {
        predicate = predicate.And(x => x.Deleted == false);
        return await repository.FirstAsync(predicate, token);
    }

    public async Task<List<TEntity>> GetAllAsync(FindOptions<TEntity>? findOptions = null, CancellationToken token = default)
    {
        return await repository.GetAllAsync(findOptions, token);
    }

    public async Task<TID> DeleteAsync(TID id)
    {
        return await repository.DeleteAsync(id);
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<TEntity> entities)
    {
        return await repository.DeleteRangeAsync(entities);
    }

    public void Dispose()
    {
        repository.Dispose();
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token)
    {
        return await repository.ExistsAsync(predicate, token);
    }

    public async Task<TID> InsertAsync(TEntity entity)
    {
        return await repository.InsertAsync(entity);
    }

    public async Task<bool> InsertRangeAsync(IEnumerable<TEntity> entities)
    {
        return await repository.InsertRangeAsync(entities);
    }

    public async Task<IEnumerable<string>> NavigationsAsync(CancellationToken token = default)
    {
        return await repository.NavigationsAsync(token);
    }

    public async Task<TID> UpdateAsync(TEntity entity)
    {
        return await repository.UpdateAsync(entity);
    }

    public async Task<bool> UpdateBatchAsync(IEnumerable<TEntity> entities)
    {
        return await repository.UpdateBatchAsync(entities);
    }
}