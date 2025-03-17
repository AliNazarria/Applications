using Applications.Usecase.Common.Interfaces;
using Applications.Usecase.Common.Models;
using Applications.Usecase.Common.Security;
using ErrorOr;
using FluentValidation;
using domain = Applications.Domain.Application;

namespace Applications.Usecase.Application.Queries;

[Authorize(Permissions = Permissions.Application.Report, Policies = Policy.Guest, Roles = "")]
public record ReportApplicationQuery(ReportFilterDTO Filter)
    : IAuthorizeableRequest<ErrorOr<PaginatedListDTO<domain.Application>>>
{
}

public class ReportApplicationQueryValidator :
    AbstractValidator<ReportApplicationQuery>
{
    public ReportApplicationQueryValidator()
    {
        //todo => filter validation !!!!!
    }
}
