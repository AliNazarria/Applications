using Applications.Usecase.Application.Interfaces;
using Common.Usecase.Dto;
using Common.Usecase.Interfaces;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Applications.Infrastructure.Persist.Repository;

public class LocalApplicationRepository(
    AppDbContext dbContext,
    IApplicationMapper mapper
    ) : IApplicationRepository
{
    public async Task<ErrorOr<List<ApplicationDTO>>> ApplicationGetlistAsync()
    {
        var report = await dbContext.Applications
            .AsNoTrackingWithIdentityResolution()
            .Include(x => x.Services)
            .ToListAsync();

        return mapper.ToDto(report);
    }
    public async Task<ErrorOr<bool>> IsExistAsync(int applicationId)
    {
        return await dbContext.Applications
           .AsNoTrackingWithIdentityResolution()
           .AnyAsync(x => x.ID == applicationId);
    }
}