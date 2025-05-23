using Common.Usecase.Interfaces;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Applications.Infrastructure.Persist.Repository;

public class LocalApplicationRepository(
    AppDbContext dbContext
    ) : IApplicationRepository
{
    public async Task<ErrorOr<List<Common.Domain.Entities.Application>>> ApplicationGetlistAsync()
    {
        return await dbContext.Applications
            .AsNoTrackingWithIdentityResolution()
            .Where(x => x.Active == true && x.Deleted == false)
            .ToListAsync();
    }
    public async Task<ErrorOr<bool>> IsExistAsync(int applicationId)
    {
        return await dbContext.Applications
           .AsNoTrackingWithIdentityResolution()
           .AnyAsync(x => x.ID == applicationId
                           && x.Active == true
                           && x.Deleted == false);
    }
}