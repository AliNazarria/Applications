using Applications.API.Api;
using Applications.API.Common;
using Applications.API.Util;
using Applications.Infrastructure;
using Applications.Usecase;
using Asp.Versioning;
using Asp.Versioning.Builder;

var versions = new Dictionary<string, string>() { { "v1", "Application" } };
var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.RegisterCommonAPI(versions);
    builder.Services.RegisterInfrastructure(builder.Configuration, builder.Environment.IsDevelopment());
}

var app = builder.Build();
{
    app.UseCommonAPI(versions,app.Environment.IsDevelopment());
    app.UseCheckRequiredHeaderParameter(ApiResources.ApiBasePath);
    app.UseHealth();

    ApiVersionSet apiVersionSet = app.NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        //.HasApiVersion(new ApiVersion(2))
        .ReportApiVersions()
        .Build();

    app.UseApplicationEndpoints(apiVersionSet);
    app.UseServiceEndpoints(apiVersionSet);
    app.UseApplicationServiceEndpoints(apiVersionSet);
    //invoice

    app.Services.EnsurePersistAndMigrate();
    app.UseUsecase();

    app.Run();
}