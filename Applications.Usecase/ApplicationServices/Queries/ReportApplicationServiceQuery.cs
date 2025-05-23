using FluentValidation;

namespace Applications.Usecase.ApplicationServices.Queries;

[Authorize(Permissions = Application.Permissions.Application.Report, Policies = Policy.Guest, Roles = "")]
public record ReportApplicationServiceQuery(ReportFilterDTO? Filter, int Page, int Size)
    : IAuthorizeableRequest<ErrorOr<PaginatedListDTO<appDomain.ApplicationService>>>
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