using Applications.Usecase.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Applications.Infrastructure.Persist.Repository;

public class ApplicationRepository(AppDbContext dbContext) : IApplicationRepository
{
    public async Task<bool> IsUnique(int id, string key)
    {
        return !await dbContext.Applications.AnyAsync(x => x.ID != id && x.Key.Value == key);
    }
}
