using Applications.Usecase.Application.Dto;
using Applications.Usecase.Application.Interfaces;
using Applications.Usecase.Application.Specifications;

namespace Applications.Usecase.Application.Queries;

public class GetAllApplicationServiceHandler(
    IGenericRepository<appDomain.ApplicationService, int> repository,
    IApplicationMapper mapper
    ) : IRequestHandler<GetAllApplicationServiceQuery, ErrorOr<List<ApplicationServiceDTO>>>
{
    public async Task<ErrorOr<List<ApplicationServiceDTO>>> Handle(GetAllApplicationServiceQuery request, CancellationToken cancellationToken)
    {
        var reportSpec = new ReportApplicationServiceSpecification();
        var report = await repository.ReportAsync(reportSpec, cancellationToken);
        return mapper.ToDto(report.Items);
    }
}