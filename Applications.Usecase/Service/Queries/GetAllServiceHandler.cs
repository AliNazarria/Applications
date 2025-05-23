namespace Applications.Usecase.Service.Queries;

public class GetAllServiceHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<serviceDomain.Service, int> repository
    ) : IRequestHandler<GetAllServiceQuery, ErrorOr<List<serviceDomain.Service>>>
{
    public async Task<ErrorOr<List<serviceDomain.Service>>> Handle(GetAllServiceQuery request, CancellationToken cancellationToken)
    {
        var options = FindOptions<serviceDomain.Service>.ReportOptions();
        return await repository.GetAllAsync(options, cancellationToken);
    }
}