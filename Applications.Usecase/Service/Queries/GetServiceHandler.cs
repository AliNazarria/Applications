using Applications.Usecase.Common;
using Applications.Usecase.Common.Interfaces;
using domain = Applications.Domain.Service;

namespace Applications.Usecase.Service.Queries;

public class GetServiceHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<domain.Service, int> repository
    ) : IRequestHandler<GetServiceQuery, ErrorOr<domain.Service>>
{
    public async Task<ErrorOr<domain.Service>> Handle(GetServiceQuery request, CancellationToken cancellationToken)
    {
        var opt = FindOptions<domain.Service>.ReportOptions();
        var result = await repository.GetAsync(request.ID, findOptions: opt, token: cancellationToken);
        if (result is null)
            return Errors.ServiceNotFound();

        return result;
    }
}