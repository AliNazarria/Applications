using Applications.API.Common;
using Applications.API.Util;
using Applications.Usecase.Common.Models;
using Applications.Usecase.Service.Commands;
using Applications.Usecase.Service.Queries;
using Asp.Versioning.Builder;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Applications.API.Api;

public static class Service
{
    public static void UseServiceEndpoints(this IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        RouteGroupBuilder appActions = app
        .MapGroup($"{ApiResources.ApiBasePath}/v{{version:apiVersion}}/{ApiResources.Services}")
        .WithTags($"{ApiResources.Services}")
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);

        appActions.MapPut("/{id:int}", ServiceUpdate);
        appActions.MapPost("/", ServiceAdd);
        appActions.MapPost("/getlist/{page:int}/{size:int}", ServiceReport);
        appActions.MapGet("/{id:int}", ServiceGet).WithName(RouteNames.ServiceGet.ToString());
        appActions.MapDelete("/{id:int}", ServiceDelete);
    }

    public static async Task<IResult> ServiceGet(
        [FromRoute] int id,
        CancellationToken token,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new GetServiceQuery(id), token);
        return responseHelper.OkResult(result, ToDto);
    }
    public static async Task<IResult> ServiceReport(
        [FromBody] ReportFilterDTO? filter,
        [FromRoute] int page,
        [FromRoute] int size,
        CancellationToken token,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new ReportServiceQuery(filter, page, size), token);
        return responseHelper.OkResult(result, ToDto);
    }
    public static async Task<IResult> ServiceAdd(
    [FromBody] ServiceInputDTO service,
    CancellationToken token,
    IEndpointLinkGenerator linkGenerator,
    HttpContext context,
    IMediator mediator,
    IResponseHelper responseHelper)
    {
        ErrorOr<int> result = await mediator.Send(new AddServiceCommand(
              service.Key, service.Name, service.Active), token);
        var location = linkGenerator.Url(context, RouteNames.ServiceGet, new { id = result.Value });
        return responseHelper.CreatedResult(result, () => { return result.Value; }, location);
    }
    public static async Task<IResult> ServiceUpdate(
        [FromBody] ServiceInputDTO service,
        [FromRoute] int id,
        CancellationToken token,
        IEndpointLinkGenerator linkGenerator,
        HttpContext context,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        ErrorOr<int> result = await mediator.Send(new UpdateServiceCommand(
             id, service.Key, service.Name, service.Active), token);
        var location = linkGenerator.Url(context, RouteNames.ServiceGet, new { id = result.Value });
        return responseHelper.CreatedResult(result, () => { return result.Value; }, location);
    }
    public static async Task<IResult> ServiceDelete(
        [FromRoute] int id,
        CancellationToken token,
        IEndpointLinkGenerator linkGenerator,
        HttpContext context,
        IMediator mediator,
        IResponseHelper responseHelper)
    {
        var result = await mediator.Send(new DeleteServiceCommand(id), token);
        var location = linkGenerator.Url(context, RouteNames.ServiceDeletedGet, new { id = result.Value });
        return responseHelper.CreatedResult(result, () => { return result.Value; }, location);
    }
    private static ServiceParamDTO ToDto(Domain.Service.Service service) =>
        new(service.ID, service.Key.Value, service.Name.Value)
        {
            Active = service.Active,
            Deleted = service.Deleted,
            Created_At = service.Created_At,
            Created_By = service.Created_By,
            Updated_At = service.Updated_At,
            Updated_By = service.Updated_By,
        };
    private static PaginatedListDTO<ServiceParamDTO> ToDto(PaginatedListDTO<Domain.Service.Service> service) =>
        new PaginatedListDTO<ServiceParamDTO>(service.Items.ConvertAll(ToDto), service.TotalCount, service.PageNumber, service.Items.Count);
}

public record ServiceInputDTO(
    string Key,
    string Name)
    : BaseInputDTO();
public record ServiceParamDTO(
    int ID,
    string Key,
    string Name
) : BaseParamDTO();