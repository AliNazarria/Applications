using Applications.API.Util;
using Asp.Versioning;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace Applications.API.Common;

public static class Injection
{
    public static IServiceCollection RegisterCommonAPI(this IServiceCollection services
        , Dictionary<string, string> versions)
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
        services.AddProblemDetails();
        services.RegisterHeaderCheck();
        return services;
    }
    public static IServiceCollection RegisterHeaderCheck(this IServiceCollection services)
    {
        services.AddScoped<CheckRequiredHeaderParameter>();
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
    public static IApplicationBuilder UseCommonAPI(this IApplicationBuilder app
        , Dictionary<string, string> versions, bool isDevelopment)
    {
        app.UseExceptionHandler("/Error");
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

        app.UseHttpsRedirection();

        return app;
    }
}