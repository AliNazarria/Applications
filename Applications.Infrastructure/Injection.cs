using Applications.Infrastructure.Persist;
using Applications.Infrastructure.Persist.Repository;
using Applications.Usecase;
using Common.Infrastructure;
using Common.Usecase.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Applications.Infrastructure;

public static class Injection
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        services.AddOptions<ConnectionStringsConfigOptions>()
            .Bind(configuration.GetSection(ConnectionStringsConfigOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .RegisterCommonInfraServices(configuration, isDevelopment)
            .RegisterLocalApplicationRepository()
            .RegisterPersist(configuration)
            .RegisterUsecase(isDevelopment);

        services.AddHybridCache(options =>
        {
            options.MaximumPayloadBytes = 1024 * 1024;
            options.MaximumKeyLength = 1024;
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(60),
                LocalCacheExpiration = TimeSpan.FromMinutes(60)
            };
        });

        services.AddScoped<IEventBus, RabbitMqEventBus>();

        return services;
    }

    private static IServiceCollection RegisterLocalApplicationRepository(this IServiceCollection services)
    {
        services.AddKeyedScoped(typeof(IApplicationRepository), Common.Usecase.Constants.Real, typeof(LocalApplicationRepository));
        services.AddKeyedScoped(typeof(IApplicationRepository), Common.Usecase.Constants.Cached, typeof(LocalApplicationRepositoryCacheProxy));
        return services;
    }
    private static IServiceCollection RegisterPersist(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();

        var option = new TypedConfiguration<ConnectionStringsConfigOptions>(configuration.GetSection(ConnectionStringsConfigOptions.SectionName));
        services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = option.Value.Redis;
            })
            .AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(option.Value.Database);
            }, ServiceLifetime.Scoped);
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

        return services;
    }
    public static void EnsurePersistAndMigrate(this IServiceProvider app)
    {
        using (var serviceScope = app.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            var databaseCreator = context.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            if (!databaseCreator.Exists())
                databaseCreator.Create();
            //context.Database.EnsureCreated();

            MigrationExt.MigrateUp(context.Database.GetConnectionString());
        }
    }
}