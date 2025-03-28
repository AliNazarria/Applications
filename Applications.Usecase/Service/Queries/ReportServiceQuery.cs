using Applications.Usecase.Common.Interfaces;
using Applications.Usecase.Common.Models;
using Applications.Usecase.Common.Security;
using ErrorOr;
using FluentValidation;
using domain = Applications.Domain.Service;

namespace Applications.Usecase.Service.Queries;

[Authorize(Permissions = Permissions.Service.Report, Policies = Policy.Guest, Roles = "")]
public record ReportServiceQuery(ReportFilterDTO? Filter, int Page, int Size)
    : IAuthorizeableRequest<ErrorOr<PaginatedListDTO<domain.Service>>>
{
}

public class ReportServiceQueryValidator :
    AbstractValidator<ReportServiceQuery>
{
    public ReportServiceQueryValidator()
    {
        //todo => filter validation !!!!!
    }
}