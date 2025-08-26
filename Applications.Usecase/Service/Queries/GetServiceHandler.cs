using Applications.Usecase.Service.Dto;
using Applications.Usecase.Service.Interfaces;
using Applications.Usecase.Service.Specifications;

namespace Applications.Usecase.Service.Queries;

public class GetServiceHandler(
    IGenericRepository<serviceDomain.Service, int> repository,
    IServiceMapper mapper
    ) : IRequestHandler<GetServiceQuery, ErrorOr<ServiceDTO>>
{
    public async Task<ErrorOr<ServiceDTO>> Handle(GetServiceQuery request, CancellationToken cancellationToken)
    {
        var getSpec = new GetServiceSpecification(request.ID);
        var result = await repository.GetAsync(getSpec, cancellationToken);
        if (result is null)
            return ServiceErrors.ServiceNotFound();

        return mapper.ToDto( result);
    }
}