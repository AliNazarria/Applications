using Applications.Usecase.Common.Interfaces;
using Applications.Usecase.Common.Security;
using ErrorOr;
using FluentValidation;
using domain = Applications.Domain.Application;

namespace Applications.Usecase.Application.Queries;

[Authorize(Permissions = Permissions.Application.Get, Policies = Policy.Guest, Roles = "")]
public record GetApplicationQuery(int ID)
    : IAuthorizeableRequest<ErrorOr<domain.Application>>
{
}

public class GetApplicationQueryValidator
    : AbstractValidator<GetApplicationQuery>
{
    public GetApplicationQueryValidator()
    {
        RuleFor(x => x.ID)
            .GreaterThan(0)
            .WithMessage(Resources.ResourceKey.IdInvalid);
    }
}