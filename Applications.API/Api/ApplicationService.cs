using Applications.Usecase.Application.Queries;
using Asp.Versioning.Builder;
using Common.API;
using Common.Usecase.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Applications.API.Api;

public class ApplicationService : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        RouteGroupBuilder appActions = app
        .MapGroup($"{ApiResources.ApiBasePath}/v{{version:apiVersion}}/{ApiResources.ApplicationServices}")
        .WithTags($"{ApiResources.ApplicationServices}")
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);

        appActions.MapPost("/getlist/{page:int}/{size:int}", ApplicationServiceReport);
        appActions.MapGet("/getall", ApplicationServiceGetAll);
    }

    private async Task<IResult> ApplicationServiceReport(
        [FromBody] ReportFilterDTO? filter,
        [FromRoute] int page,
        [FromRoute] int size,
        CancellationToken token,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new ReportApplicationServiceQuery(filter, page, size), token);
        return responseHelper.OkResult(result);
    }
    private async Task<IResult> ApplicationServiceGetAll(
        CancellationToken token,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new GetAllApplicationServiceQuery(), token);
        return responseHelper.OkResult(result);
    }
}