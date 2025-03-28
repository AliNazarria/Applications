using Applications.Usecase.Common.Interfaces;
using ErrorOr;
using MediatR;
using domain = Applications.Domain.Application;

namespace Applications.Usecase.Application.Queries;

public class GetApplicationHandler(
    IGenericRepository<domain.Application, int> repository
    )
    : IRequestHandler<GetApplicationQuery, ErrorOr<domain.Application>>
{
    async Task<ErrorOr<domain.Application>> IRequestHandler<GetApplicationQuery, ErrorOr<domain.Application>>.Handle(
        GetApplicationQuery request, CancellationToken cancellationToken)
    {
        var option = FindOptions<domain.Application>.ReportOptions();
        var result = await repository.GetAsync(request.ID, findOptions: option, token: cancellationToken);
        if (result is null || result.Deleted)
            return Error.NotFound(description: Resources.ResourceKey.Application.NotFound);

        return result;
    }
}