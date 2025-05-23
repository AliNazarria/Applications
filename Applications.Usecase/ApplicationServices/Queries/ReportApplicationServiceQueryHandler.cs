namespace Applications.Usecase.ApplicationServices.Queries;

public class ReportApplicationServiceQueryHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<appDomain.ApplicationService, int> repository
    )
    : IRequestHandler<ReportApplicationServiceQuery, ErrorOr<PaginatedListDTO<appDomain.ApplicationService>>>
{
    public async Task<ErrorOr<PaginatedListDTO<appDomain.ApplicationService>>> Handle(ReportApplicationServiceQuery request, CancellationToken cancellationToken)
    {
        var predicateResult = PredicateBuilder.MakePredicate<appDomain.ApplicationService>(request.Filter?.Filter);
        if (predicateResult.IsError)
            return predicateResult.Errors;

        var options = FindOptions<appDomain.ApplicationService>.ReportOptions();
        var result = await repository.GetPagedAsync(predicateResult.Value
            , request.Filter?.OrderBy
            , request.Page
            , request.Size
            , findOptions: options
            , token: cancellationToken);
        return result;
    }
}