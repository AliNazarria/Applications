using Applications.Usecase.Application;
using Applications.Usecase.Application.Interfaces;
using Applications.Usecase.Service;
using Applications.Usecase.Service.Interfaces;
using FluentValidation;

namespace Applications.Usecase;

public static class Injection
{
    public static IServiceCollection RegisterUsecase(this IServiceCollection services, bool isDevelopment)
    {
        services.AddSingleton<IApplicationMapper, ApplicationMapper>();
        services.AddSingleton<IServiceMapper, ServiceMapper>();

        services.RegisterCommonUsecaseServices(typeof(Injection).Assembly, isDevelopment);
        services.AddValidatorsFromAssemblyContaining(typeof(Injection));

        return services;
    }
}