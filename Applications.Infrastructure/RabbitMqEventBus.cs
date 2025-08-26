using Applications.Usecase;
using Common.Infrastructure;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;

namespace Applications.Infrastructure;

public class RabbitMqEventBus(
    IOptions<ConnectionStringsConfigOptions> options,
    HybridCache hybridCache
    ) : IEventBus
{
    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
    {
        await hybridCache.RemoveByTagAsync([
            CacheConstants.ApplicationsTag,
            CacheConstants.ApplicationExistTag
            ]);
    }
}