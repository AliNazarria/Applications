using Applications.API.Common;
using Applications.API.Util;
using Applications.Usecase.Application.Queries;
using Applications.Usecase.Application.Commands;
using Applications.Usecase.Common.Models;
using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Applications.API;

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
        appActions.MapGet("/{id:int}", AppGet).WithName(RouteNames.ApplicationSet);
        appActions.MapDelete("/{id:int}", AppDelete);
    }

    public static async Task<ResponseDTO<ApplicationParamDTO>> AppGet([FromRoute] int id, CancellationToken token, IMediator mediator)
    {
        var result = await mediator.Send(new GetApplicationQuery(id), token);
        return ResponseHelper.ToDto<Domain.Application.Application, ApplicationParamDTO>(result, ToDto);
        //return result.Match(
        //    app => Results.Ok(ToDto(app))
        //    , error => Results.NotFound(error)
        //    );
    }
    public static async Task<ResponseDTO<PaginatedListDTO<ApplicationParamDTO>>> AppReport([FromBody] ReportFilterDTO filter, CancellationToken token, IMediator mediator)
    {
        var result = await mediator.Send(new ReportApplicationQuery(filter), token);
        return ResponseHelper.ToDto<PaginatedListDTO<Domain.Application.Application>, PaginatedListDTO<ApplicationParamDTO>>(result, ToDto);
        //return result.Match(
        // response => Results.Ok(ToDto(response))
        // , errors => Results.NotFound(errors)
        // );
    }
    public static async Task<ResponseDTO<int>> AppSet([FromBody] ApplicationParamDTO app, CancellationToken token, IMediator mediator)
    {
        if (app.ID > 0)
        {
            var result = await mediator.Send(new UpdateApplicationCommand(
                app.ID, app.Key, app.Title, app.Comment, app.LogoAddress, app.Active), token);
            return ResponseHelper.ToResult<int, int>(result, () => { return result.Value; });
        }
        else
        {
            var result = await mediator.Send(new AddApplicationCommand(
              app.Key, app.Title, app.Comment, app.LogoAddress, app.Active), token);
            return ResponseHelper.ToResult<int, int>(result, () => { return result.Value; });
        }
        //return Results.CreatedAtRoute(RouteNames.ApplicationSet, new { id = result.Value }, result.Value);
        //.Created<int>($"/{ApiResources.Applications}/{result.Value}",result.Value);
    }
    public static async Task<ResponseDTO<int>> AppDelete([FromRoute] int id, CancellationToken token, IMediator mediator)
    {
        var result = await mediator.Send(new DeleteApplicationCommand(id), token);
        return ResponseHelper.ToResult<int, int>(result, () => { return result.Value; });

        //return Results.Created<long>($"/{ApiResources.Applications}/{resultId}", id);
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