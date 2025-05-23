using Applications.API.Util;
using Applications.Usecase.ApplicationServices.Commands;
using Applications.Usecase.ApplicationServices.Queries;
using Asp.Versioning.Builder;
using Common.API;
using Common.Usecase.Models;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Applications.API.Api;

public class ApplicationService: IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        RouteGroupBuilder appActions = app
        .MapGroup($"{ApiResources.ApiBasePath}/v{{version:apiVersion}}/{ApiResources.ApplicationServices}")
        .WithTags($"{ApiResources.ApplicationServices}")
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);

        appActions.MapPut("/{id:int}", ApplicationServiceUpdate);
        appActions.MapPost("/", ApplicationServiceAdd);
        appActions.MapPost("/getlist/{page:int}/{size:int}", ApplicationServiceReport);
        appActions.MapGet("/getall", ApplicationServiceGetAll);
        appActions.MapDelete("/{id:int}", ApplicationServiceDelete);
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
        return responseHelper.OkResult(result, ToDto);
    }
    private async Task<IResult> ApplicationServiceGetAll(
        CancellationToken token,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new GetAllApplicationServiceQuery(), token);
        return responseHelper.OkResult(result, ToDto);
    }
    private async Task<IResult> ApplicationServiceAdd(
          [FromBody] ApplicationServiceInputDTO app,
          CancellationToken token,
          IEndpointLinkGenerator linkGenerator,
          HttpContext context,
          IMediator mediator,
          IResponseHelper responseHelper)
    {
        ErrorOr<int> result = await mediator.Send(new AddApplicationServiceCommand(
           app.ApplicationID, app.ServiceID, app.Active), token);

        var location = linkGenerator.PathByName(context, RouteNames.ApplicationServiceGet.ToString(), new { id = result.Value });
        return responseHelper.CreatedResult(result, () => { return result.Value; }, location);
    }
    private async Task<IResult> ApplicationServiceUpdate(
        [FromBody] ApplicationServiceInputDTO app,
        [FromRoute] int id,
        CancellationToken token,
        IEndpointLinkGenerator linkGenerator,
        HttpContext context,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        ErrorOr<int> result = await mediator.Send(new UpdateApplicationServiceCommand(
             id, app.ApplicationID, app.ServiceID, app.Active), token);

        var location = linkGenerator.PathByName(context, RouteNames.ApplicationServiceGet.ToString(), new { id = result.Value });
        return responseHelper.CreatedResult(result, () => { return result.Value; }, location);
    }
    private async Task<IResult> ApplicationServiceDelete(
        [FromRoute] int id,
        CancellationToken token,
        IEndpointLinkGenerator linkGenerator,
        HttpContext context,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new DeleteApplicationServiceCommand(id), token);
        var location = linkGenerator.PathByName(context, RouteNames.ApplicationServiceDeletedGet.ToString(), new { id = result.Value });
        return responseHelper.CreatedResult(result, () => { return result.Value; }, location);
    }
    private static ApplicationServiceParamDTO ToDto(Common.Domain.Entities.ApplicationService service) =>
        new(service.ID, service.ApplicationID, service.ServiceID)
        {
            Active = service.Active,
            Deleted = service.Deleted,
            Created_At = service.Created_At,
            Created_By = service.Created_By,
            Updated_At = service.Updated_At,
            Updated_By = service.Updated_By,
        };
    private static List<ApplicationServiceParamDTO> ToDto(List<Common.Domain.Entities.ApplicationService> service) =>
        service.ConvertAll(ToDto);
    private static PaginatedListDTO<ApplicationServiceParamDTO> ToDto(PaginatedListDTO<Common.Domain.Entities.ApplicationService> app) =>
        new PaginatedListDTO<ApplicationServiceParamDTO>(app.Items.ConvertAll(ToDto), app.TotalCount, app.PageNumber, app.Items.Count);
}

public record ApplicationServiceInputDTO(
    int ApplicationID,
    int ServiceID)
    : BaseInputDTO();
public record ApplicationServiceParamDTO(
    int ID,
    int ApplicationID,
    int ServiceID
) : BaseParamDTO();