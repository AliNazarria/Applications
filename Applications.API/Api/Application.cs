using Applications.API.Common;
using Applications.API.Util;
using Applications.Usecase.Application.Commands;
using Applications.Usecase.Application.Queries;
using Applications.Usecase.Common.Models;
using Asp.Versioning.Builder;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Applications.API.Api;

public static class Application//: IEndpointRouteHandlerBuilder
{
    public static void UseApplicationEndpoints(this IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        RouteGroupBuilder appActions = app
        .MapGroup($"{ApiResources.ApiBasePath}/v{{version:apiVersion}}/{ApiResources.Applications}")
        .WithTags($"{ApiResources.Applications}")
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);

        appActions.MapPut("/", AppSet);
        appActions.MapPost("/", AppReport);
        appActions.MapGet("/{id:int}", AppGet).WithName(RouteNames.ApplicationGet.ToString());
        appActions.MapDelete("/{id:int}", AppDelete);
    }

    public static async Task<IResult> AppGet(
        [FromRoute] int id,
        CancellationToken token,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new GetApplicationQuery(id), token);
        return responseHelper.OkResult(result, ToDto);
    }
    public static async Task<IResult> AppReport(
        [FromBody] ReportFilterDTO filter,
        CancellationToken token,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new ReportApplicationQuery(filter), token);
        return responseHelper.OkResult(result, ToDto);
    }
    public static async Task<IResult> AppSet(
        [FromBody] ApplicationParamDTO app,
        CancellationToken token,
        IEndpointLinkGenerator linkGenerator,
        HttpContext context,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        ErrorOr<int> result;
        if (app.ID > 0)
        {
            result = await mediator.Send(new UpdateApplicationCommand(
                app.ID, app.Key, app.Title, app.Comment, app.LogoAddress, app.Active), token);
        }
        else
        {
            result = await mediator.Send(new AddApplicationCommand(
              app.Key, app.Title, app.Comment, app.LogoAddress, app.Active), token);
        }

        var location = linkGenerator.Url(context,RouteNames.ApplicationGet, new { id = result.Value });
        return responseHelper.CreatedResult(result, () => { return result.Value; }, location);
    }
    public static async Task<IResult> AppDelete(
        [FromRoute] int id,
        CancellationToken token,
        IEndpointLinkGenerator linkGenerator,
        HttpContext context,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new DeleteApplicationCommand(id), token);
        var location = linkGenerator.Url(context, RouteNames.ApplicationDeletedGet, new { id = result.Value });
        return responseHelper.CreatedResult(result, () => { return result.Value; }, location);
    }
    private static ApplicationParamDTO ToDto(Domain.Application.Application app) =>
        new(app.ID, app.Key.Value, app.Title.Value, app.Comment.Value, app.LogoAddress.Value)
        {
            Active = app.Active,
            Deleted = app.Deleted,
            Created_At = app.Created_At,
            Created_By = app.Created_By,
            Updated_At = app.Updated_At,
            Updated_By = app.Updated_By,
        };
    private static PaginatedListDTO<ApplicationParamDTO> ToDto(PaginatedListDTO<Domain.Application.Application> app) =>
        new PaginatedListDTO<ApplicationParamDTO>(app.Items.ConvertAll(ToDto), app.TotalCount, app.PageNumber, app.Items.Count);
}

public record ApplicationParamDTO(
    int ID,
    string Key,
    string Title,
    string? Comment,
    string? LogoAddress
) : BaseParamDTO();