using Applications.API.Common;
using Applications.API.Util;
using Applications.Usecase.ApplicationServices.Commands;
using Applications.Usecase.ApplicationServices.Queries;
using Applications.Usecase.Common.Models;
using Asp.Versioning.Builder;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Applications.API.Api;

public static class ApplicationService
{
    public static void UseApplicationServiceEndpoints(this IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        RouteGroupBuilder appActions = app
        .MapGroup($"{ApiResources.ApiBasePath}/v{{version:apiVersion}}/{ApiResources.ApplicationServices}")
        .WithTags($"{ApiResources.ApplicationServices}")
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);

        appActions.MapPut("/", ApplicationServiceSet);
        appActions.MapPost("/", ApplicationServiceReport);
        appActions.MapDelete("/{id:int}", ApplicationServiceDelete);
    }

    public static async Task<IResult> ApplicationServiceReport(
        [FromBody] ReportFilterDTO filter,
        CancellationToken token,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new ReportApplicationServiceQuery(filter), token);
        return responseHelper.OkResult(result, ToDto);
    }
    public static async Task<IResult> ApplicationServiceSet(
        [FromBody] ApplicationServiceParamDTO app,
        CancellationToken token,
        IEndpointLinkGenerator linkGenerator,
        HttpContext context,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        ErrorOr<int> result;
        if (app.ID > 0)
        {
            result = await mediator.Send(new UpdateApplicationServiceCommand(
                app.ID, app.ApplicationID, app.ServiceID, app.Active), token);
        }
        else
        {
            result = await mediator.Send(new AddApplicationServiceCommand(
              app.ApplicationID, app.ServiceID, app.Active), token);
        }
        var location = linkGenerator.Url(context, RouteNames.ApplicationServiceGet, new { id = result.Value });
        return responseHelper.CreatedResult(result, () => { return result.Value; }, location);
    }
    public static async Task<IResult> ApplicationServiceDelete(
        [FromRoute] int id,
        CancellationToken token,
        IEndpointLinkGenerator linkGenerator,
        HttpContext context,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new DeleteApplicationServiceCommand(id), token);
        var location = linkGenerator.Url(context, RouteNames.ApplicationServiceDeletedGet, new { id = result.Value });
        return responseHelper.CreatedResult(result, () => { return result.Value; }, location);
    }
    private static ApplicationServiceParamDTO ToDto(Domain.Application.ApplicationService service) =>
        new(service.ID, service.ApplicationID, service.ServiceID)
        {
            Active = service.Active,
            Deleted = service.Deleted,
            Created_At = service.Created_At,
            Created_By = service.Created_By,
            Updated_At = service.Updated_At,
            Updated_By = service.Updated_By,
        };
    private static PaginatedListDTO<ApplicationServiceParamDTO> ToDto(PaginatedListDTO<Domain.Application.ApplicationService> app) =>
        new PaginatedListDTO<ApplicationServiceParamDTO>(app.Items.ConvertAll(ToDto), app.TotalCount, app.PageNumber, app.Items.Count);
}

public record ApplicationServiceParamDTO(
    int ID,
    int ApplicationID,
    int ServiceID
) : BaseParamDTO();
