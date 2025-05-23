using Applications.Usecase.Application;
using Applications.Usecase.Service;
using FluentValidation;

namespace Applications.Usecase.ApplicationServices.Commands;

[Authorize(Permissions = Application.Permissions.Application.Update, Policies = Policy.Admin, Roles = Roles.Admin)]
public record AddApplicationServiceCommand(
    int ApplicationID,
    int ServiceID,
    bool Active) : IAuthorizeableRequest<ErrorOr<int>>
{
}

public class AddApplicationServiceCommandValidator
    : AbstractValidator<AddApplicationServiceCommand>
{
    public AddApplicationServiceCommandValidator(
        IDateTimeProvider dateTimeProvider)
    {
        RuleFor(x => x.ApplicationID).ApplicationId();
        RuleFor(x => x.ServiceID).ServiceId();
    }
}