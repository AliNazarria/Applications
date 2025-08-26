using Applications.Usecase.Application.Dto;
using Applications.Usecase.Application.Interfaces;
using Applications.Usecase.Application.Specifications;

namespace Applications.Usecase.Application.Queries;

public class ReportApplicationServiceQueryHandler(
    IGenericRepository<appDomain.ApplicationService, int> repository,
    IApplicationMapper mapper)
    : IRequestHandler<ReportApplicationServiceQuery, ErrorOr<PaginatedListDTO<ApplicationServiceDTO>>>
{
    public async Task<ErrorOr<PaginatedListDTO<ApplicationServiceDTO>>> Handle(ReportApplicationServiceQuery request, CancellationToken cancellationToken)
    {
        var reportSpec = new ReportApplicationServiceSpecification(request.Page, request.Size, request.Filter);
        var report = await repository.ReportAsync(reportSpec, cancellationToken);
        return mapper.ToDto(report);
    }
}