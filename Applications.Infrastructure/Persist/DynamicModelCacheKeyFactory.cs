using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Applications.Infrastructure.Persist;

public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
{
    public object Create(DbContext context, bool designTime)
    {
        if (context is AppDbContext dbContext)
        {
            var isAdmin = dbContext.CurrentIsAdmin;
            return (context.GetType(), isAdmin);
        }

        return context.GetType();
    }
}