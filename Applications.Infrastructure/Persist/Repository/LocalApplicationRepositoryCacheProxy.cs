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
    public async Task<ErrorOr<List<Common.Domain.Entities.Application>>> ApplicationGetlistAsync()
    {
        CancellationToken token = default;
        var key = "applications";
        var tags = new List<string> { key };
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
        var tags = new List<string> { key };
        return await hybridCache.GetOrCreateAsync(
             key,
             async get =>
             {
                 var result = await applicationRepository.IsExistAsync(applicationId);
                 return result.Value;
             },
             tags: tags,
             cancellationToken: token);
    }
}