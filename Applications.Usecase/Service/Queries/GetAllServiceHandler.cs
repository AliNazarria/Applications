using Applications.Usecase.Service.Dto;
using Applications.Usecase.Service.Interfaces;
using Applications.Usecase.Service.Specifications;

namespace Applications.Usecase.Service.Queries;

public class GetAllServiceHandler(
    IGenericRepository<serviceDomain.Service, int> repository,
    IServiceMapper mapper
    ) : IRequestHandler<GetAllServiceQuery, ErrorOr<List<ServiceDTO>>>
{
    public async Task<ErrorOr<List<ServiceDTO>>> Handle(GetAllServiceQuery request, CancellationToken cancellationToken)
    {
        var reportSpec = new ReportServiceSpecification();
        var report = await repository.ReportAsync(reportSpec, cancellationToken);
        return mapper.ToDto(report.Items);
    }
}