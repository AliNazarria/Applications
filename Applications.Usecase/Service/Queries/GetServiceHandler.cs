namespace Applications.Usecase.Service.Queries;

public class GetServiceHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<serviceDomain.Service, int> repository
    ) : IRequestHandler<GetServiceQuery, ErrorOr<serviceDomain.Service>>
{
    public async Task<ErrorOr<serviceDomain.Service>> Handle(GetServiceQuery request, CancellationToken cancellationToken)
    {
        var opt = FindOptions<serviceDomain.Service>.ReportOptions();
        var result = await repository.GetAsync(request.ID, findOptions: opt, token: cancellationToken);
        if (result is null)
            return ServiceErrors.ServiceNotFound();

        return result;
    }
}