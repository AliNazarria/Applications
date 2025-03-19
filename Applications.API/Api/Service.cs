using Applications.API.Common;
using Applications.API.Util;
using Applications.Usecase.Common.Models;
using Applications.Usecase.Service.Commands;
using Applications.Usecase.Service.Queries;
using Asp.Versioning.Builder;
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

        appActions.MapPut("/", ServiceSet);
        appActions.MapPost("/", ServiceReport);
        appActions.MapGet("/{id:int}", ServiceGet);
        appActions.MapDelete("/{id:int}", ServiceDelete);
    }

    public static async Task<ResponseDTO<ServiceParamDTO>> ServiceGet(
        [FromRoute] int id, CancellationToken token, IMediator mediator)
    {
        var result = await mediator.Send(new GetServiceQuery(id), token);
        return ResponseHelper.ToDto(result, ToDto);
    }
    public static async Task<ResponseDTO<PaginatedListDTO<ServiceParamDTO>>> ServiceReport(
        [FromBody] ReportFilterDTO filter, CancellationToken token, IMediator mediator)
    {
        var result = await mediator.Send(new ReportServiceQuery(filter), token);
        return ResponseHelper.ToDto(result, ToDto);
    }
    public static async Task<ResponseDTO<int>> ServiceSet(
        [FromBody] ServiceParamDTO service, CancellationToken token, IMediator mediator)
    {
        if (service.ID > 0)
        {
            var result = await mediator.Send(new UpdateServiceCommand(
                service.ID, service.Key, service.Name, service.Active), token);
            return ResponseHelper.ToResult(result, () => { return result.Value; });
        }
        else
        {
            var result = await mediator.Send(new AddServiceCommand(
              service.Key, service.Name, service.Active), token);
            return ResponseHelper.ToResult(result, () => { return result.Value; });
        }
    }
    public static async Task<ResponseDTO<int>> ServiceDelete(
        [FromRoute] int id, CancellationToken token, IMediator mediator)
    {
        var result = await mediator.Send(new DeleteServiceCommand(id), token);
        return ResponseHelper.ToResult(result, () => { return result.Value; });
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

public record ServiceParamDTO(
    int ID,
    string Key,
    string Name
) : BaseParamDTO();