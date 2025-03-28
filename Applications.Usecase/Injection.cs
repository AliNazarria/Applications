using Applications.Usecase.Common.Behaviors;
using Applications.Usecase.Common.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Applications.Usecase;

public static class Injection
{
    public static IServiceCollection RegisterUsecase(this IServiceCollection services, bool isDevelopment)
    {
        services.RegisterCommonServices(typeof(Injection).Assembly, isDevelopment);
        services.AddValidatorsFromAssemblyContaining(typeof(Injection));

        return services;
    }
    public static IServiceCollection RegisterCommonServices(this IServiceCollection services, Assembly assembly, bool isDevelopment)
    {
        services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(assembly);
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
            options.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
        });
        return services;
    }
}