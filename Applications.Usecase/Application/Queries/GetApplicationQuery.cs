using FluentValidation;

namespace Applications.Usecase.Application.Queries;

[Authorize(Permissions = Permissions.Application.Get, Policies = Policy.Guest, Roles = "")]
public record GetApplicationQuery(int ID)
    : IAuthorizeableRequest<ErrorOr<appDomain.Application>>
{
}

public class GetApplicationQueryValidator
    : AbstractValidator<GetApplicationQuery>
{
    public GetApplicationQueryValidator()
    {
        RuleFor(x => x.ID).ApplicationId();
    }
}