namespace Applications.Usecase.Application.Queries;

public class GetAllAplicationHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<appDomain.Application, int> repository
    ) : IRequestHandler<GetAllAplicationQuery, ErrorOr<List<appDomain.Application>>>
{
    public async Task<ErrorOr<List<appDomain.Application>>> Handle(GetAllAplicationQuery request, CancellationToken cancellationToken)
    {
        var options = FindOptions<appDomain.Application>.ReportOptions();
        return await repository.GetAllAsync(options, cancellationToken);
    }
}