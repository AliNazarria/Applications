namespace Applications.Usecase.ApplicationServices.Queries;

public class GetAllApplicationServiceHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<appDomain.ApplicationService, int> repository
    ) : IRequestHandler<GetAllApplicationServiceQuery, ErrorOr<List<appDomain.ApplicationService>>>
{
    public async Task<ErrorOr<List<appDomain.ApplicationService>>> Handle(GetAllApplicationServiceQuery request, CancellationToken cancellationToken)
    {
        var options = FindOptions<appDomain.ApplicationService>.ReportOptions();
        return await repository.GetAllAsync(options, cancellationToken);
    }
}