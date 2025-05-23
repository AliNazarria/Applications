using Common.Usecase;
using FluentValidation;

namespace Applications.Usecase;

public static class Injection
{
    public static IServiceCollection RegisterUsecase(this IServiceCollection services, bool isDevelopment)
    {
        services.RegisterCommonUsecaseServices(typeof(Injection).Assembly, isDevelopment);
        services.AddValidatorsFromAssemblyContaining(typeof(Injection));

        return services;
    }

}