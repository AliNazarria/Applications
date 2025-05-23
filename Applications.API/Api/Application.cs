using Applications.API.Util;
using Applications.Usecase.Application.Commands;
using Applications.Usecase.Application.Queries;
using Asp.Versioning.Builder;
using Common.API;
using Common.Usecase.Models;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Applications.API.Api;

public class Application: IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        RouteGroupBuilder appActions = app
        .MapGroup($"{ApiResources.ApiBasePath}/v{{version:apiVersion}}/{ApiResources.Applications}")
        .WithTags($"{ApiResources.Applications}")
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);

        appActions.MapPut("/{id:int}", AppUpdate);
        appActions.MapPost("/", AppAdd);
        appActions.MapPost("/getlist/{page:int}/{size:int}", AppReport);
        appActions.MapGet("/getall", AppGetAll);
        appActions.MapGet("/{id:int}", AppGet).WithName(RouteNames.ApplicationGet.ToString());
        appActions.MapDelete("/{id:int}", AppDelete);
    }

    private async Task<IResult> AppGet(
        [FromRoute] int id,
        CancellationToken token,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new GetApplicationQuery(id), token);
        return responseHelper.OkResult(result, ToDto);
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
        return responseHelper.OkResult(result, ToDto);
    }
    private async Task<IResult> AppGetAll(
        CancellationToken token,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new GetAllAplicationQuery(), token);
        return responseHelper.OkResult(result, ToDto);
    }
    private async Task<IResult> AppAdd(
        [FromBody] ApplicationInputDTO app,
        CancellationToken token,
        IEndpointLinkGenerator linkGenerator,
        HttpContext context,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        ErrorOr<int> result = await mediator.Send(new AddApplicationCommand(
              app.Key, app.Title, app.Comment, app.LogoAddress, app.Active), token);
        var location = linkGenerator.PathByName(context, RouteNames.ApplicationGet.ToString(), new { id = result.Value });
        return responseHelper.CreatedResult(result, () => { return result.Value; }, location);
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
        ErrorOr<int> result = await mediator.Send(new UpdateApplicationCommand(
               id, app.Key, app.Title, app.Comment, app.LogoAddress, app.Active), token);
        var location = linkGenerator.PathByName(context, RouteNames.ApplicationGet.ToString(), new { id = result.Value });
        return responseHelper.CreatedResult(result, () => { return result.Value; }, location);
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
        return responseHelper.CreatedResult(result, () => { return result.Value; }, location);
    }
    private static ApplicationParamDTO ToDto(Common.Domain.Entities.Application app) =>
        new(app.ID, app.Key.Value, app.Title.Value, app.Comment.Value, app.LogoAddress.Value)
        {
            Active = app.Active,
            Deleted = app.Deleted,
            Created_At = app.Created_At,
            Created_By = app.Created_By,
            Updated_At = app.Updated_At,
            Updated_By = app.Updated_By,
        };
    private static List<ApplicationParamDTO> ToDto(List<Common.Domain.Entities.Application> app) =>
        app.ConvertAll(ToDto);
    private static PaginatedListDTO<ApplicationParamDTO> ToDto(PaginatedListDTO<Common.Domain.Entities.Application> app) =>
        new PaginatedListDTO<ApplicationParamDTO>(app.Items.ConvertAll(ToDto), app.TotalCount, app.PageNumber, app.Items.Count);
}

public record ApplicationInputDTO(
    string Key,
    string Title,
    string? Comment,
    string? LogoAddress) : BaseInputDTO();
public record ApplicationParamDTO(
    int ID,
    string Key,
    string Title,
    string? Comment,
    string? LogoAddress
) : BaseParamDTO();