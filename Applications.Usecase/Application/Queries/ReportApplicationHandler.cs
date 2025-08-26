using Applications.Usecase.Application.Interfaces;
using Applications.Usecase.Application.Specifications;

namespace Applications.Usecase.Application.Queries;

public class ReportApplicationHandler(
    IGenericRepository<appDomain.Application, int> repository,
    IApplicationMapper mapper)
    : IRequestHandler<ReportApplicationQuery, ErrorOr<PaginatedListDTO<ApplicationDTO>>>
{
    public async Task<ErrorOr<PaginatedListDTO<ApplicationDTO>>> Handle(
        ReportApplicationQuery request, CancellationToken cancellationToken)
    {
        var reportSpec = new ReportApplicationSpecification(request.Page, request.Size, request.Filter);
        var report = await repository.ReportAsync(reportSpec, cancellationToken);
        return mapper.ToDto(report);
    }
}