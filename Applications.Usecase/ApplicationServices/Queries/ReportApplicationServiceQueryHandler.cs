using Applications.Domain.Application;
using Applications.Usecase.Common;
using Applications.Usecase.Common.Interfaces;
using Applications.Usecase.Common.Models;
using domain = Applications.Domain.Application;

namespace Applications.Usecase.ApplicationServices.Queries;

public class ReportApplicationServiceQueryHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<domain.ApplicationService, int> repository
    )
    : IRequestHandler<ReportApplicationServiceQuery, ErrorOr<PaginatedListDTO<domain.ApplicationService>>>
{
    public async Task<ErrorOr<PaginatedListDTO<ApplicationService>>> Handle(ReportApplicationServiceQuery request, CancellationToken cancellationToken)
    {
        var predicateResult = PredicateBuilder.MakePredicate<domain.ApplicationService>(request.Filter.Filter);
        if (predicateResult.IsError)
            return predicateResult.Errors;

        var opt = FindOptions<domain.ApplicationService>.ReportOptions();
        var result = await repository.GetPagedAsync(predicateResult.Value
            , request.Filter.OrderBy
            , request.Page
            , request.Size
            , findOptions: opt
            , token: cancellationToken);
        return result;
    }
}