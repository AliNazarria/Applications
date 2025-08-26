using Applications.Usecase.Application.Commands;
using Applications.Usecase.Application.Dto;
using Applications.Usecase.Application.Queries;
using Asp.Versioning.Builder;
using Common.API;
using Common.Usecase.Dto;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Applications.API.Api;

public class Application : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        RouteGroupBuilder authActions = app
        .MapGroup($"{ApiResources.AuthBasePath}/v{{version:apiVersion}}/{ApiResources.Applications}")
        .WithTags($"{ApiResources.Applications}")
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);
        authActions.MapGet("/getall", AppGetAll);

        RouteGroupBuilder appActions = app
        .MapGroup($"{ApiResources.ApiBasePath}/v{{version:apiVersion}}/{ApiResources.Applications}")
        .WithTags($"{ApiResources.Applications}")
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);

        appActions.MapPut("/{id:int}", AppUpdate);
        appActions.MapPost("/", AppAdd);
        appActions.MapPost("/getlist/{page:int}/{size:int}", AppReport);
        appActions.MapGet("/{id:int}", AppGet).WithName(RouteNames.ApplicationGet.ToString());
        appActions.MapDelete("/{id:int}", AppDelete);

        appActions.MapPost("/{applicationId:int}/service", ApplicationServiceAdd);
        appActions.MapPut("/{applicationId:int}/service/{id:int}", ApplicationServiceUpdate);
        appActions.MapDelete("/{applicationId:int}/service/{id:int}", ApplicationServiceDelete);
    }

    private async Task<IResult> ApplicationServiceAdd(
          [FromRoute] int applicationId,
          [FromBody] ApplicationServiceInputDTO app,
          CancellationToken token,
          IEndpointLinkGenerator linkGenerator,
          HttpContext context,
          IMediator mediator,
          IResponseHelper responseHelper)
    {
        ErrorOr<int> result = await mediator.Send(new AddApplicationServiceCommand(applicationId, app), token);
        var location = linkGenerator.PathByName(context, RouteNames.ApplicationGet.ToString(), new { id = result.Value });
        return responseHelper.CreatedResult(result, location);
    }
    private async Task<IResult> ApplicationServiceUpdate(
        [FromBody] ApplicationServiceInputDTO app,
        [FromRoute] int applicationId,
        [FromRoute] int id,
        CancellationToken token,
        IEndpointLinkGenerator linkGenerator,
        HttpContext context,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        ErrorOr<int> result = await mediator.Send(new UpdateApplicationServiceCommand(applicationId, id, app), token);
        var location = linkGenerator.PathByName(context, RouteNames.ApplicationGet.ToString(), new { id = result.Value });
        return responseHelper.CreatedResult(result, location);
    }
    private async Task<IResult> ApplicationServiceDelete(
        [FromRoute] int applicationId,
        [FromRoute] int id,
        CancellationToken token,
        IEndpointLinkGenerator linkGenerator,
        HttpContext context,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        ErrorOr<int> result = await mediator.Send(new DeleteApplicationServiceCommand(applicationId, id), token);
        var location = linkGenerator.PathByName(context, RouteNames.ApplicationGet.ToString(), new { id = result.Value });
        return responseHelper.CreatedResult(result, location);
    }

    private async Task<IResult> AppGet(
        [FromRoute] int id,
        CancellationToken token,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new GetApplicationQuery(id), token);
        return responseHelper.OkResult(result);
    }
    private async Task<IResult> AppReport(
        [FromBody] ReportFilterDTO? filter,
        [FromRoute] int page,
        [FromRoute] int size,
        CancellationToken token,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new ReportApplicationQuery(filter, page, size), token);
        return responseHelper.OkResult(result);
    }
    private async Task<IResult> AppGetAll(
        CancellationToken token,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new GetAllAplicationQuery(), token);
        return responseHelper.OkResult(result);
    }
    private async Task<IResult> AppAdd(
        [FromBody] ApplicationInputDTO app,
        CancellationToken token,
        IEndpointLinkGenerator linkGenerator,
        HttpContext context,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        ErrorOr<int> result = await mediator.Send(new AddApplicationCommand(app), token);
        var location = linkGenerator.PathByName(context, RouteNames.ApplicationGet.ToString(), new { id = result.Value });
        return responseHelper.CreatedResult(result, location);
    }
    private async Task<IResult> AppUpdate(
        [FromBody] ApplicationInputDTO app,
        [FromRoute] int id,
        CancellationToken token,
        IEndpointLinkGenerator linkGenerator,
        HttpContext context,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        ErrorOr<int> result = await mediator.Send(new UpdateApplicationCommand(id, app), token);
        var location = linkGenerator.PathByName(context, RouteNames.ApplicationGet.ToString(), new { id = result.Value });
        return responseHelper.CreatedResult(result, location);
    }
    private async Task<IResult> AppDelete(
        [FromRoute] int id,
        CancellationToken token,
        IEndpointLinkGenerator linkGenerator,
        HttpContext context,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new DeleteApplicationCommand(id), token);
        var location = linkGenerator.PathByName(context, RouteNames.ApplicationDeletedGet.ToString(), new { id = result.Value });
        return responseHelper.CreatedResult(result, location);
    }
}