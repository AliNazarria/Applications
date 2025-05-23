using FluentValidation;


namespace Applications.Usecase.Service.Queries;

[Authorize(Permissions = Permissions.Service.Get, Policies = Policy.Guest, Roles = "")]
public record GetServiceQuery(int ID)
    : IAuthorizeableRequest<ErrorOr<serviceDomain.Service>>
{
}

public class GetServiceQueryValidator
    : AbstractValidator<GetServiceQuery>
{
    public GetServiceQueryValidator()
    {
        RuleFor(x => x.ID).ServiceId();
    }
}