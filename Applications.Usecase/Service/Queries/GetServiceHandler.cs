using Applications.Usecase.Common.Interfaces;
using ErrorOr;
using MediatR;
using domain = Applications.Domain.Service;

namespace Applications.Usecase.Service.Queries;

public class GetServiceHandler(
    IGenericRepository<domain.Service, int> repository,
    IResourceLocalizer localizer
    ) : IRequestHandler<GetServiceQuery, ErrorOr<domain.Service>>
{
    public async Task<ErrorOr<domain.Service>> Handle(GetServiceQuery request, CancellationToken cancellationToken)
    {
        //todo => check deleted
        var opt = FindOptions<domain.Service>.ReportOptions();
        var result = await repository.GetAsync(request.ID, findOptions: opt, token: cancellationToken);
        if (result is domain.Service)
            return result;

        return Error.NotFound(description: localizer.Localize(Resources.ResourceKey.Service.NotFound));
    }
}