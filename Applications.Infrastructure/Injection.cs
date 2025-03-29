using Applications.Infrastructure.Common;
using Applications.Infrastructure.Persist;
using Applications.Infrastructure.Persist.Repository;
using Applications.Usecase;
using Applications.Usecase.Application.Interfaces;
using Applications.Usecase.Common.Interfaces;
using Applications.Usecase.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Applications.Infrastructure;

public static class Injection
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        services
            .RegisterCommonServices(isDevelopment)
            .RegisterPersist(configuration)
            .RegisterUsecase(isDevelopment);

        return services;
    }
    private static IServiceCollection RegisterCommonServices(this IServiceCollection services, bool isDevelopment)
    {
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddScoped<IUserContextProvider, UserContextProvider>();
        services.AddScoped<IAuthorizationServiceProvider, AuthorizationServiceProvider>();
        services.AddScoped<ILoggerServiceProvider, LoggerServiceProvider>();

        return services;
    }
    private static IServiceCollection RegisterPersist(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(PersistSettings.Section);
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        services.AddKeyedScoped(typeof(IGenericRepository<,>), "real", typeof(GenericRepository<,>));
        services.AddScoped<IApplicationRepository, ApplicationRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();

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