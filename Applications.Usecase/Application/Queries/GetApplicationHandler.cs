namespace Applications.Usecase.Application.Queries;

public class GetApplicationHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<appDomain.Application, int> repository
    )
    : IRequestHandler<GetApplicationQuery, ErrorOr<appDomain.Application>>
{
    async Task<ErrorOr<appDomain.Application>> IRequestHandler<GetApplicationQuery, ErrorOr<appDomain.Application>>.Handle(
        GetApplicationQuery request, CancellationToken cancellationToken)
    {
        var option = FindOptions<appDomain.Application>.ReportOptions();
        var result = await repository.GetAsync(request.ID, findOptions: option, token: cancellationToken);
        if (result is null)
            return ApplicationErrors.ApplicationNotFound();

        return result;
    }
}