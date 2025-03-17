using Applications.Usecase.Common.Behaviors;
using Applications.Usecase.Common.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;
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

        services.RegisterLocalization();
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(assembly);
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
            options.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
        });
        return services;
    }
    private static IServiceCollection RegisterLocalization(this IServiceCollection services)
    {
        //todo
        services.AddLocalization(opt => opt.ResourcesPath = "Resources");
        services.AddScoped<IResourceLocalizer, ResourceLocalizer>();
        services.Configure<RequestLocalizationOptions>(
        opts =>
        {
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("fa"),
                new CultureInfo("en")
            };
            opts.DefaultRequestCulture = new RequestCulture("fa");
            opts.SupportedCultures = supportedCultures;
            opts.SupportedUICultures = supportedCultures;
        });

        return services;
    }
    public static IApplicationBuilder UseUsecase(this IApplicationBuilder app)
    {
        var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
        app.UseRequestLocalization(options.Value);
        return app;
    }
}

public partial class CommonResourceKey
{
    public const string FilterInvalid = "FilterInvalid";
}
public interface IResourceLocalizer
{
    string Localize(string key);
}
public class ResourceLocalizer : IResourceLocalizer
{
    private readonly IStringLocalizer _localizer;

    public ResourceLocalizer(IStringLocalizerFactory factory, IUserContextProvider userContext)
    {
        var type = typeof(Resources.ResourceKey.Application);
        var assemblyName = new AssemblyName(type.Assembly.FullName!);
        var resoureName = string.IsNullOrWhiteSpace(userContext.Language) ? "fa" : userContext.Language;
        _localizer = factory.Create($"{resoureName}", assemblyName.Name!);
    }

    public string Localize(string key)
    {
        return _localizer[key];
    }
}