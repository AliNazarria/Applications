using Applications.Usecase.Common;
using Applications.Usecase.Common.Interfaces;
using Applications.Usecase.Common.Models;
using ErrorOr;
using MediatR;
using domain = Applications.Domain.Application;

namespace Applications.Usecase.Application.Queries;

public class ReportApplicationHandler(
    IGenericRepository<domain.Application, int> repository
    ) :
    IRequestHandler<ReportApplicationQuery, ErrorOr<PaginatedListDTO<domain.Application>>>
{
    public async Task<ErrorOr<PaginatedListDTO<domain.Application>>> Handle(
        ReportApplicationQuery request, CancellationToken cancellationToken)
    {
        var predicateResult = PredicateBuilder.MakePredicate<domain.Application>(request.Filter.Filter);
        if (predicateResult.IsError)
            return predicateResult.Errors;

        var options = FindOptions<domain.Application>.ReportOptions();
        var result = await repository.GetPagedAsync(predicateResult.Value
            , request.Filter.OrderBy
            , request.Page
            , request.Size
            , findOptions: options
            , token: cancellationToken);
        return result;
    }
}