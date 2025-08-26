using Common.Infrastructure;
using Common.Usecase.Dto;
using Common.Usecase.Interfaces;
using ErrorOr;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;

namespace Applications.Infrastructure.Persist.Repository;

public class LocalApplicationRepositoryCacheProxy(
    [FromKeyedServices(Common.Usecase.Constants.Real)] IApplicationRepository applicationRepository,
    HybridCache hybridCache
    ) : IApplicationRepository
{
    public async Task<ErrorOr<List<ApplicationDTO>>> ApplicationGetlistAsync()
    {
        CancellationToken token = default;
        var key = "applications";
        var tags = new List<string> { CacheConstants.ApplicationsTag };
        return await hybridCache.GetOrCreateAsync(
            key,
            async get =>
            {
                var result = await applicationRepository.ApplicationGetlistAsync();
                return result.Value;
            },
            //options: entryOptions,
            tags: tags,
            cancellationToken: token);
    }

    public async Task<ErrorOr<bool>> IsExistAsync(int applicationId)
    {
        CancellationToken token = default;
        var key = $"application-{applicationId}";
        var tags = new List<string> { CacheConstants.ApplicationExistTag };
        var result = await hybridCache.GetOrCreateAsync(
             key,
             async get =>
             {
                 var result = await applicationRepository.IsExistAsync(applicationId);
                 return result.Value;
             },
             tags: tags,
             cancellationToken: token);
        return result;
    }
}