using Applications.Usecase.Service.Dto;
using FluentValidation;

namespace Applications.Usecase.Service.Queries;

[Authorize(Permissions = Permissions.Service.Report, Policies = Policy.Guest, Roles = "")]
public record ReportServiceQuery(ReportFilterDTO? Filter, int Page, int Size)
    : IAuthorizeableRequest<ErrorOr<PaginatedListDTO<ServiceDTO>>>
{
}

public class ReportServiceQueryValidator :
    AbstractValidator<ReportServiceQuery>
{
    public ReportServiceQueryValidator()
    {
        RuleFor(x => x.Size).PageSize();
    }
}