using Applications.Usecase.Application.Interfaces;
using Applications.Usecase.Application.Specifications;

namespace Applications.Usecase.Application.Queries;

public class GetApplicationHandler(
    IGenericRepository<appDomain.Application, int> repository,
    IApplicationMapper mapper)
    : IRequestHandler<GetApplicationQuery, ErrorOr<ApplicationDTO>>
{
    async Task<ErrorOr<ApplicationDTO>> IRequestHandler<GetApplicationQuery, ErrorOr<ApplicationDTO>>.Handle(
        GetApplicationQuery request, CancellationToken cancellationToken)
    {
        var getSpec = new GetApplicationSpecification(request.ID);
        var result = await repository.SingleGetAsync(getSpec, cancellationToken);
        if (result is null)
            return ApplicationErrors.ApplicationNotFound();

        return mapper.ToDto(result);
    }
}