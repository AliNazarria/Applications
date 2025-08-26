using Applications.Usecase.Application.Dto;
using FluentValidation;

namespace Applications.Usecase.Application.Queries;

[Authorize(Permissions = Permissions.Application.Report, Policies = Policy.Guest, Roles = "")]
public record ReportApplicationServiceQuery(ReportFilterDTO? Filter, int Page, int Size)
    : IAuthorizeableRequest<ErrorOr<PaginatedListDTO<ApplicationServiceDTO>>>
{
}

public class ReportApplicationServiceQueryValidator :
    AbstractValidator<ReportApplicationServiceQuery>
{
    public ReportApplicationServiceQueryValidator()
    {
        RuleFor(x => x.Size).PageSize();
    }
}