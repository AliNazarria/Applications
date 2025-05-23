namespace Applications.Usecase.Application.Queries;

public class ReportApplicationHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<appDomain.Application, int> repository
    ) :
    IRequestHandler<ReportApplicationQuery, ErrorOr<PaginatedListDTO<appDomain.Application>>>
{
    public async Task<ErrorOr<PaginatedListDTO<appDomain.Application>>> Handle(
        ReportApplicationQuery request, CancellationToken cancellationToken)
    {
        var predicateResult = PredicateBuilder.MakePredicate<appDomain.Application>(request.Filter?.Filter);
        if (predicateResult.IsError)
            return predicateResult.Errors;

        var options = FindOptions<appDomain.Application>.ReportOptions();
        var result = await repository.GetPagedAsync(predicateResult.Value
            , request.Filter?.OrderBy
            , request.Page
            , request.Size
            , findOptions: options
            , token: cancellationToken);
        return result;
    }
}