using Applications.Infrastructure;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Common.API;

var versions = new Dictionary<string, string>() { { "v1", "Application" } };
var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.RegisterCommonAPI(builder.Configuration, versions);
    builder.Services.RegisterInfrastructure(builder.Configuration, builder.Environment.IsDevelopment());
}

var app = builder.Build();
{
    app.UseCommonAPI(versions, app.Environment.IsDevelopment());
    app.UseCheckRequiredHeaderParameter();
    app.UseHealth();

    ApiVersionSet apiVersionSet = app.NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        //.HasApiVersion(new ApiVersion(2))
        .ReportApiVersions()
        .Build();

    app.RegisterAllEndpoints(apiVersionSet);
    app.Services.EnsurePersistAndMigrate();
    app.Run();
}