using Applications.Usecase.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Applications.Infrastructure.Persist.Repository;

public class ServiceRepository(AppDbContext dbContext) : IServiceRepository
{
    public async Task<bool> IsUnique(int id, string key)
    {
        return !await dbContext.Services.AnyAsync(x => x.ID != id && x.Key.Value == key);
    }
}