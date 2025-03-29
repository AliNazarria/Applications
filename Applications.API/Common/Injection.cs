using Asp.Versioning;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Applications.API.Common;

public static class Injection
{
    private static string CorsPolicyName = "AllowMyOrigins";
    public static IServiceCollection RegisterCommonAPI(this IServiceCollection services,
        IConfiguration configuration,
        Dictionary<string, string> versions)
    {
        var versionBuilder = services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new QueryStringApiVersionReader("version"),
                new UrlSegmentApiVersionReader(),
                //new HeaderApiVersionReader("X-API-Version"),
                new MediaTypeApiVersionReader("version")
                );
        });
        versionBuilder.AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddHttpContextAccessor();
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<AddRequiredHeaderParameter>();

            foreach (var ver in versions)
            {
                options.SwaggerDoc(ver.Key, new OpenApiInfo { Title = ver.Value, Version = ver.Key });
            }
            options.ResolveConflictingActions(apiDescriptions =>
            {
                return apiDescriptions.First();
            });
        });
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.RegisterHeaderCheck();
        services.RegisterLocalization();
        services.AddScoped<IResponseHelper, ResponseHelper>();
        services.AddScoped<IEndpointLinkGenerator, EndpointLinkGenerator>();
        services.AddCors(options =>
        {
            string[] allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [];
            options.AddPolicy(CorsPolicyName, policy => policy
            .WithOrigins(allowedOrigins)
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod());
        });
        return services;
    }
    private static IServiceCollection RegisterHeaderCheck(this IServiceCollection services)
    {
        services.AddScoped<CheckRequiredHeaderParameter>();
        return services;
    }
    private static IServiceCollection RegisterLocalization(this IServiceCollection services)
    {
        services.AddSingleton<LocalizationMiddleware>();
        services.AddDistributedMemoryCache();
        services.AddLocalization();
        services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo("fa"),
                new CultureInfo("en")
            };
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });

        return services;
    }

    public static void UseHealth(this WebApplication app, string address = "/health")
    {
        app.MapGet(address, () => Results.Ok());
    }
    public static void UseCheckRequiredHeaderParameter(this IApplicationBuilder app, string apiBaseAddress)
    {
        app.UseWhen(
            context => context.Request.Path.StartsWithSegments($"/{apiBaseAddress}"),
            builder => builder.UseMiddleware<CheckRequiredHeaderParameter>());
    }
    public static IApplicationBuilder UseCommonAPI(this IApplicationBuilder app,
         Dictionary<string, string> versions,
         bool isDevelopment)
    {
        app.UseExceptionHandler();
        //app.UseExceptionHandler("/Error");
        if (isDevelopment)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var ver in versions)
                {
                    options.SwaggerEndpoint($"/swagger/{ver.Key}/swagger.json", $"{ver.Key}");
                }
            });
        }

        var supportedCultures = new[] { "fa", "en" };
        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);
        localizationOptions.ApplyCurrentCultureToResponseHeaders = true;
        app.UseRequestLocalization(localizationOptions);
        app.UseMiddleware<LocalizationMiddleware>();
        app.UseCors(CorsPolicyName);
        app.UseHttpsRedirection();

        return app;
    }
}