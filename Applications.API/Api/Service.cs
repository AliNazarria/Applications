using Applications.Usecase.Service.Commands;
using Applications.Usecase.Service.Dto;
using Applications.Usecase.Service.Queries;
using Asp.Versioning.Builder;
using Common.API;
using Common.Usecase.Dto;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Applications.API.Api;

public class Service : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        RouteGroupBuilder appActions = app
        .MapGroup($"{ApiResources.ApiBasePath}/v{{version:apiVersion}}/{ApiResources.Services}")
        .WithTags($"{ApiResources.Services}")
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);

        appActions.MapPut("/{id:int}", ServiceUpdate);
        appActions.MapPost("/", ServiceAdd);
        appActions.MapPost("/getlist/{page:int}/{size:int}", ServiceReport);
        appActions.MapGet("/getall", ServiceGetAll);
        appActions.MapGet("/{id:int}", ServiceGet).WithName(RouteNames.ServiceGet.ToString());
        appActions.MapDelete("/{id:int}", ServiceDelete);
    }

    private async Task<IResult> ServiceGet(
        [FromRoute] int id,
        CancellationToken token,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new GetServiceQuery(id), token);
        return responseHelper.OkResult(result);
    }
    private async Task<IResult> ServiceReport(
        [FromBody] ReportFilterDTO? filter,
        [FromRoute] int page,
        [FromRoute] int size,
        CancellationToken token,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new ReportServiceQuery(filter, page, size), token);
        return responseHelper.OkResult(result);
    }
    private async Task<IResult> ServiceGetAll(
        CancellationToken token,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new GetAllServiceQuery(), token);
        return responseHelper.OkResult(result);
    }

    private async Task<IResult> ServiceAdd(
        [FromBody] ServiceInputDTO service,
        CancellationToken token,
        IEndpointLinkGenerator linkGenerator,
        HttpContext context,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        ErrorOr<int> result = await mediator.Send(new AddServiceCommand(service), token);
        var location = linkGenerator.PathByName(context, RouteNames.ServiceGet.ToString(), new { id = result.Value });
        return responseHelper.CreatedResult(result, location);
    }
    private async Task<IResult> ServiceUpdate(
        [FromBody] ServiceInputDTO service,
        [FromRoute] int id,
        CancellationToken token,
        IEndpointLinkGenerator linkGenerator,
        HttpContext context,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        ErrorOr<int> result = await mediator.Send(new UpdateServiceCommand(id, service), token);
        var location = linkGenerator.PathByName(context, RouteNames.ServiceGet.ToString(), new { id = result.Value });
        return responseHelper.CreatedResult(result, location);
    }
    private async Task<IResult> ServiceDelete(
        [FromRoute] int id,
        CancellationToken token,
        IEndpointLinkGenerator linkGenerator,
        HttpContext context,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new DeleteServiceCommand(id), token);
        var location = linkGenerator.PathByName(context, RouteNames.ServiceDeletedGet.ToString(), new { id = result.Value });
        return responseHelper.CreatedResult(result, location);
    }
}