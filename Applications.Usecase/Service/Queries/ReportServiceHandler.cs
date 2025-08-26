using Applications.Usecase.Service.Dto;
using Applications.Usecase.Service.Interfaces;
using Applications.Usecase.Service.Specifications;

namespace Applications.Usecase.Service.Queries;

public class ReportServiceHandler(
    IGenericRepository<serviceDomain.Service, int> repository,
    IServiceMapper mapper
    ) : IRequestHandler<ReportServiceQuery, ErrorOr<PaginatedListDTO<ServiceDTO>>>
{
    public async Task<ErrorOr<PaginatedListDTO<ServiceDTO>>> Handle(ReportServiceQuery request, CancellationToken cancellationToken)
    {
        var reportSpec = new ReportServiceSpecification(request.Page, request.Size, request.Filter);
        var report = await repository.ReportAsync(reportSpec, cancellationToken);
        return mapper.ToDto(report);
    }
}