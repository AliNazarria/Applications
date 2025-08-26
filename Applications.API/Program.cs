using Applications.API.HealthChecks;
using Applications.Infrastructure;
using Applications.Infrastructure.Persist;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Common.API;
using Common.Usecase;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

var versions = new Dictionary<string, string>() { { "v1", "Application" } };
string healthCheckTags = "checker";

var builder = WebApplication.CreateBuilder(args);
{
    builder.Host.UseSerilog((context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    });

    builder.Services.AddHealthChecks()
        .AddCheck<SqlHealthCheck>(nameof(SqlHealthCheck), HealthStatus.Unhealthy, tags: [healthCheckTags])
        .AddDbContextCheck<AppDbContext>();
    builder.Services.Configure<HealthCheckPublisherOptions>(options =>
    {
        var delay = Int32.Parse(builder.Configuration["HealthCheck:DelayFromMinute"]);
        options.Delay = TimeSpan.FromMinutes(delay > 0 ? delay : 10);
        options.Predicate = healthCheck => healthCheck.Tags.Contains(healthCheckTags);
    });
    builder.Services.AddSingleton<IHealthCheckPublisher, HealthCheckPublisher>();

    builder.Services.RegisterCommonAPI(builder.Configuration, versions);
    builder.Services.RegisterInfrastructure(builder.Configuration, builder.Environment.IsDevelopment());
}

var app = builder.Build();
{
    app.UseCommonAPI(versions, app.Environment.IsDevelopment());
    app.UseCheckRequiredHeaderParameter();
    app.UseHealth();
    app.UseCommonUsecase(app.Environment.IsDevelopment());

    ApiVersionSet apiVersionSet = app.NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        //.HasApiVersion(new ApiVersion(2))
        .ReportApiVersions()
        .Build();

    app.RegisterAllEndpoints(apiVersionSet);
    app.Services.EnsurePersistAndMigrate();
    app.Run();
}