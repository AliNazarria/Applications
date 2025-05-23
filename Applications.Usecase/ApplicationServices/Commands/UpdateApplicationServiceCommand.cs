using Applications.Usecase.Application;
using Applications.Usecase.Service;
using FluentValidation;

namespace Applications.Usecase.ApplicationServices.Commands;

[Authorize(Permissions = Application.Permissions.Application.Update, Policies = Policy.Admin, Roles = Roles.Admin)]
public record UpdateApplicationServiceCommand(
    int ID,
    int ApplicationID,
    int ServiceID,
    bool Active
    )
    : IAuthorizeableRequest<ErrorOr<int>>
{
}

public class UpdateApplicationServiceCommandValidator :
    AbstractValidator<UpdateApplicationServiceCommand>
{
    public UpdateApplicationServiceCommandValidator()
    {
        RuleFor(x => x.ID).ApplicationServiceId();
        RuleFor(x => x.ApplicationID).ApplicationId();
        RuleFor(x => x.ServiceID).ServiceId();
    }
}