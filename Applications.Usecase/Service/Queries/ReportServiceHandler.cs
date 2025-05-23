namespace Applications.Usecase.Service.Queries;

public class ReportServiceHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<serviceDomain.Service, int> repository
    )
    : IRequestHandler<ReportServiceQuery, ErrorOr<PaginatedListDTO<serviceDomain.Service>>>
{
    public async Task<ErrorOr<PaginatedListDTO<serviceDomain.Service>>> Handle(ReportServiceQuery request, CancellationToken cancellationToken)
    {
        var predicateResult = PredicateBuilder.MakePredicate<serviceDomain.Service>(request.Filter?.Filter);
        if (predicateResult.IsError)
            return predicateResult.Errors;

        var options = FindOptions<serviceDomain.Service>.ReportOptions();
        var result = await repository.GetPagedAsync(predicateResult.Value
            , request.Filter?.OrderBy
            , request.Page
            , request.Size
            , findOptions: options
            , token: cancellationToken);
        return result;
    }
}