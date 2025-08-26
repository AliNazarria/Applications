using Common.Domain;
using Common.Infrastructure;
using Common.Usecase.Dto;
using Common.Usecase.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Applications.Infrastructure.Persist.Repository;

public class GenericRepository<TEntity, TID>
    : IGenericRepository<TEntity, TID>
    where TEntity : Entity<TID>
{
    protected readonly AppDbContext dbContext;
    public readonly IUserContextProvider userContext;
    private bool _disposed;

    public GenericRepository(AppDbContext dbcontext, IUserContextProvider usercontext)
    {
        dbContext = dbcontext;
        userContext = usercontext;
    }

    public async Task<PaginatedListDTO<TEntity>> ReportAsync(ISpecification<TEntity> spec,
        CancellationToken cancellationToken = default)
    {
        var query = SpecificationEvaluator<TEntity>.GetQuery(dbContext.Set<TEntity>().AsQueryable(), spec);
        var items = await query.ToListAsync(cancellationToken);

        var countQuery = SpecificationEvaluator<TEntity>.GetQuery(dbContext.Set<TEntity>().AsQueryable(), spec,
            ignorePaging: true);
        var count = await countQuery.CountAsync();

        return new PaginatedListDTO<TEntity>(items, count, spec.Page, spec.Size);
    }
    public async Task<TEntity> GetAsync(ISpecification<TEntity> spec,
        CancellationToken cancellationToken = default)
    {
        var query = SpecificationEvaluator<TEntity>.GetQuery(dbContext.Set<TEntity>().AsQueryable(), spec);
        return await query.FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<TEntity> SingleGetAsync(ISpecification<TEntity> spec,
        CancellationToken token = default)
    {
        var query = SpecificationEvaluator<TEntity>.GetQuery(dbContext.Set<TEntity>().AsQueryable(), spec);
        return await query.SingleOrDefaultAsync(token);
    }
    public async Task<int> CountAsync(ISpecification<TEntity> spec,
        CancellationToken token = default)
    {
        return await dbContext.Set<TEntity>().CountAsync(spec.Criteria);
    }
    public async Task<bool> ExistsAsync(ISpecification<TEntity> spec,
        CancellationToken token = default)
    {
        return await dbContext.Set<TEntity>().CountAsync(spec.Criteria) != 0;
    }
    public async Task<IEnumerable<string>> NavigationsAsync(CancellationToken token = default)
    {
        var entityNavigations = new List<string>();
        foreach (var property in dbContext.Model.FindEntityType(typeof(TEntity)).GetNavigations())
            entityNavigations.Add(property.Name);

        return entityNavigations;
    }
    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        dbContext.Set<TEntity>().Add(entity);
        var result = await dbContext.SaveChangesAsync();
        return result > 0 ? entity : null;
    }
    public async Task<bool> InsertRangeAsync(IEnumerable<TEntity> entities)
    {
        dbContext.Set<TEntity>().AddRangeAsync(entities);
        var result = await dbContext.SaveChangesAsync();
        return result > 0;
    }
    public async Task<bool> UpdateAsync(TEntity entity)
    {
        dbContext.Set<TEntity>().Attach(entity);
        dbContext.Entry(entity).State = EntityState.Modified;
        var result = await dbContext.SaveChangesAsync();
        return result > 0;
    }
    public async Task<bool> UpdateBatchAsync(IEnumerable<TEntity> entities)
    {
        var keys = dbContext.Model.FindEntityType(typeof(TEntity)).GetKeys();
        var primaryKeys = keys.SelectMany(x => x.Properties).Where(x => x.IsPrimaryKey() == true).Select(x => x.Name).ToList();
        foreach (var entity in entities)
        {
            var value = TypeDescriptor.GetProperties(entity)[primaryKeys.FirstOrDefault()].GetValue(entity);
            var entry = dbContext.Set<TEntity>().Find(value);
            if (entry == null)
                await dbContext.Set<TEntity>().AddAsync(entity);
            else
            {
                dbContext.Set<TEntity>().Attach(entity);
                dbContext.Entry(entity).State = EntityState.Modified;
            }
        }
        var result = await dbContext.SaveChangesAsync();
        return result > 0;
    }
    public async Task<bool> DeleteAsync(TID id)
    {
        var entity = await dbContext.Set<TEntity>().FindAsync(id);
        if (entity == null)
        {
            return default;
        }
        dbContext.Set<TEntity>().Remove(entity);
        var result = await dbContext.SaveChangesAsync();
        return result > 0;
    }
    public async Task<bool> DeleteRangeAsync(IEnumerable<TEntity> entities)
    {
        dbContext.Set<TEntity>().RemoveRange(entities);
        var result = await dbContext.SaveChangesAsync();
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
                dbContext.Dispose();
            }
        }
        _disposed = true;
    }
}