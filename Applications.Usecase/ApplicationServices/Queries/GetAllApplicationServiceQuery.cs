using FluentValidation;

namespace Applications.Usecase.ApplicationServices.Queries;

[Authorize(Permissions = Application.Permissions.Application.Report, Policies = Policy.Guest, Roles = "")]
public record GetAllApplicationServiceQuery()
    : IAuthorizeableRequest<ErrorOr<List<appDomain.ApplicationService>>>
{
}

public class GetAllApplicationServiceQueryValidator
    : AbstractValidator<GetAllApplicationServiceQuery>
{
    public GetAllApplicationServiceQueryValidator()
    {

    }
}