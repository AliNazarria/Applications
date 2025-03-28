using Applications.Infrastructure.Common;
using Applications.Usecase.Common.Interfaces;
using Applications.Usecase.Common.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Applications.Infrastructure.Persist.Repository;

public class GenericRepository<TEntity, TID>
    : IGenericRepository<TEntity, TID>
    where TEntity : Entity<TID>
{
    protected readonly AppDbContext _dbContext;
    private bool _disposed;

    public GenericRepository(AppDbContext context)
    {
        _dbContext = context;
    }

    public async Task<TEntity> GetAsync(TID id
        , FindOptions<TEntity>? findOptions = null
        , CancellationToken token = default)
    {
        return await Get(findOptions).FindAsync(id);
    }
    public async Task<PaginatedListDTO<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>> predicate
        , string orderBy
        , int page
        , int size
        , FindOptions<TEntity>? findOptions = null
        , CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        size = size < 1 ? 1 : size;
        page = page < 1 ? 1 : page;

        var orderedQuery = Get(findOptions).Where(predicate).OrderBy(orderBy);
        var items = await orderedQuery.Skip((page - 1) * size).Take(size).ToListAsync();
        var count = await orderedQuery.CountAsync();
        return new PaginatedListDTO<TEntity>(items, count, page, size);
    }
    public async Task<List<TEntity>> GetAllAsync(
        FindOptions<TEntity>? findOptions = null
        , CancellationToken token = default)
    {
        return await Get(findOptions).ToListAsync();
    }

    public async Task<IEnumerable<string>> NavigationsAsync(CancellationToken token = default)
    {
        var entityNavigations = new List<string>();
        foreach (var property in _dbContext.Model.FindEntityType(typeof(TEntity)).GetNavigations())
            entityNavigations.Add(property.Name);

        return entityNavigations;
    }
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate
        , CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return await _dbContext.Set<TEntity>().CountAsync(predicate);
    }
    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate
        , CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return await _dbContext.Set<TEntity>().CountAsync(predicate) != 0;
    }
    public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate
        , CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }

    public async Task<TID> InsertAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Add(entity);
        var result = await _dbContext.SaveChangesAsync();
        return entity.ID;
    }
    public async Task<bool> InsertRangeAsync(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().AddRangeAsync(entities);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }
    public async Task<TID> UpdateAsync(TEntity entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        var result = await _dbContext.SaveChangesAsync();
        return entity.ID;
    }
    public async Task<bool> UpdateBatchAsync(IEnumerable<TEntity> entities)
    {
        var keys = _dbContext.Model.FindEntityType(typeof(TEntity)).GetKeys();
        var primaryKeys = keys.SelectMany(x => x.Properties).Where(x => x.IsPrimaryKey() == true).Select(x => x.Name).ToList();
        foreach (var entity in entities)
        {
            var value = TypeDescriptor.GetProperties(entity)[primaryKeys.FirstOrDefault()].GetValue(entity);
            var entry = _dbContext.Set<TEntity>().Find(value);
            if (entry == null)
                await _dbContext.Set<TEntity>().AddAsync(entity);
            else
            {
                _dbContext.Set<TEntity>().Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
        }
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }
    public async Task<TID> DeleteAsync(TID id)
    {
        var entity = await _dbContext.Set<TEntity>().FindAsync(id);
        if (entity == null)
        {
            return default;
        }
        _dbContext.Set<TEntity>().Remove(entity);
        var result = await _dbContext.SaveChangesAsync();
        return id;
    }
    public async Task<bool> DeleteRangeAsync(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().RemoveRange(entities);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
        _disposed = true;
    }

    private DbSet<TEntity> Get(FindOptions<TEntity>? findOptions = null)
    {
        findOptions ??= new FindOptions<TEntity>();
        var entity = _dbContext.Set<TEntity>();
        if (findOptions.IsIgnoreAutoIncludes)
        {
            entity.IgnoreAutoIncludes();
        }
        if (findOptions.IsAsNoTracking)
        {
            entity.AsNoTracking();
        }

        foreach (var include in findOptions.Includes)
        {
            entity.Include(include).Load();
        }
        return entity;
    }
}