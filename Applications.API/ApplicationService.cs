using Applications.API.Common;
using Applications.API.Util;
using Applications.Usecase.ApplicationServices.Commands;
using Applications.Usecase.ApplicationServices.Queries;
using Applications.Usecase.Common.Models;
using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Applications.API;

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

    public static async Task<ResponseDTO<PaginatedListDTO<ApplicationServiceParamDTO>>> ApplicationServiceReport(
        [FromBody] ReportFilterDTO filter, CancellationToken token, IMediator mediator)
    {
        var result = await mediator.Send(new ReportApplicationServiceQuery(filter), token);
        return ResponseHelper.ToDto<PaginatedListDTO<Domain.Application.ApplicationService>, PaginatedListDTO<ApplicationServiceParamDTO>>(result, ToDto);
    }
    public static async Task<ResponseDTO<int>> ApplicationServiceSet(
        [FromBody] ApplicationServiceParamDTO app, CancellationToken token, IMediator mediator)
    {
        if (app.ID > 0)
        {
            var result = await mediator.Send(new UpdateApplicationServiceCommand(
                app.ID, app.ApplicationID, app.ServiceID, app.Active), token);
            return ResponseHelper.ToResult<int, int>(result, () => { return result.Value; });
        }
        else
        {
            var result = await mediator.Send(new AddApplicationServiceCommand(
              app.ApplicationID, app.ServiceID, app.Active), token);
            return ResponseHelper.ToResult<int, int>(result, () => { return result.Value; });
        }
    }
    public static async Task<ResponseDTO<int>> ApplicationServiceDelete(
        [FromRoute] int id, CancellationToken token, IMediator mediator)
    {
        var result = await mediator.Send(new DeleteApplicationServiceCommand(id), token);
        return ResponseHelper.ToResult<int, int>(result, () => { return result.Value; });
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
