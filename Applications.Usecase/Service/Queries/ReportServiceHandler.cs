using Applications.Usecase.Common;
using Applications.Usecase.Common.Interfaces;
using Applications.Usecase.Common.Models;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using domain = Applications.Domain.Service;

namespace Applications.Usecase.Service.Queries;

public class ReportServiceHandler(
    [FromKeyedServices("proxy")] IGenericRepository<domain.Service, int> repository
    )
    : IRequestHandler<ReportServiceQuery, ErrorOr<PaginatedListDTO<domain.Service>>>
{
    public async Task<ErrorOr<PaginatedListDTO<domain.Service>>> Handle(ReportServiceQuery request, CancellationToken cancellationToken)
    {
        var predicateResult = PredicateBuilder.MakePredicate<domain.Service>(request.Filter.Filter);
        if (predicateResult.IsError)
            return predicateResult.Errors;

        var opt = FindOptions<domain.Service>.ReportOptions();
        var result = await repository.GetPagedAsync(predicateResult.Value
            , request.Filter.OrderBy
            , request.Page
            , request.Size
            , findOptions: opt
            , token: cancellationToken);
        return result;
    }
}