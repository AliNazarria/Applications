using Applications.Usecase.Common.Interfaces;
using Applications.Usecase.Common.Security;
using ErrorOr;
using FluentValidation;

namespace Applications.Usecase.ApplicationServices.Commands;

[Authorize(Permissions = Application.Permissions.Application.Update, Policies = Policy.Admin, Roles = Roles.Admin)]
public record UpdateApplicationServiceCommand(
    int ID,
    int application,
    int service,
    bool active
    )
    : IAuthorizeableRequest<ErrorOr<int>>
{
}

public class UpdateApplicationServiceCommandValidator :
    AbstractValidator<UpdateApplicationServiceCommand>
{
    public UpdateApplicationServiceCommandValidator()
    {
        RuleFor(x => x.ID)
            .GreaterThan(0)
            .WithMessage(Resources.ResourceKey.IdInvalid);
        RuleFor(x => x.application)
            .NotNull().NotEmpty().GreaterThan(0)
            .WithMessage(Resources.ResourceKey.IdInvalid);
        RuleFor(x => x.service)
            .NotNull().NotEmpty().GreaterThan(0)
            .WithMessage(Resources.ResourceKey.IdInvalid);
    }
}
