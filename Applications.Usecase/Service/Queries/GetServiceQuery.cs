using Applications.Usecase.Common.Interfaces;
using Applications.Usecase.Common.Security;
using ErrorOr;
using FluentValidation;
using domain = Applications.Domain.Service;


namespace Applications.Usecase.Service.Queries;

[Authorize(Permissions = Permissions.Service.Get, Policies = Policy.Guest, Roles = "")]
public record GetServiceQuery(int ID)
    : IAuthorizeableRequest<ErrorOr<domain.Service>>
{
}

public class GetServiceQueryValidator
    : AbstractValidator<GetServiceQuery>
{
    public GetServiceQueryValidator()
    {
        RuleFor(x => x.ID)
            .GreaterThan(0)
            .WithMessage(Resources.ResourceKey.IdInvalid);
    }
}