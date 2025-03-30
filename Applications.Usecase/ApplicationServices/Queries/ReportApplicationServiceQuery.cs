using Applications.Usecase.Common;
using Applications.Usecase.Common.Interfaces;
using Applications.Usecase.Common.Models;
using Applications.Usecase.Common.Security;
using ErrorOr;
using FluentValidation;
using domain = Applications.Domain.Application;

namespace Applications.Usecase.ApplicationServices.Queries;

[Authorize(Permissions = Application.Permissions.Application.Report, Policies = Policy.Guest, Roles = "")]
public record ReportApplicationServiceQuery(ReportFilterDTO? Filter, int Page, int Size)
    : IAuthorizeableRequest<ErrorOr<PaginatedListDTO<domain.ApplicationService>>>
{
}

public class ReportApplicationServiceQueryValidator :
    AbstractValidator<ReportApplicationServiceQuery>
{
    public ReportApplicationServiceQueryValidator()
    {
        RuleFor(x => x.Size)
            .GreaterThanOrEqualTo(Constants.MaxPageSize)
            .WithMessage(Resources.ResourceKey.PageSizeInvalid);
    }
}