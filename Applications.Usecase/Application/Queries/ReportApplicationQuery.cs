using FluentValidation;

namespace Applications.Usecase.Application.Queries;

[Authorize(Permissions = Permissions.Application.Report, Policies = Policy.Guest, Roles = "")]
public record ReportApplicationQuery(ReportFilterDTO? Filter, int Page, int Size)
    : IAuthorizeableRequest<ErrorOr<PaginatedListDTO<ApplicationDTO>>>
{
}

public class ReportApplicationQueryValidator :
    AbstractValidator<ReportApplicationQuery>
{
    public ReportApplicationQueryValidator()
    {
        RuleFor(x => x.Size).PageSize();
    }
}