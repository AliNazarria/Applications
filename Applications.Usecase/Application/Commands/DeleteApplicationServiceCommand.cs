using Applications.Usecase.Application;
using FluentValidation;

namespace Applications.Usecase.Application.Commands;

[Authorize(Permissions = Application.Permissions.Application.Update, Policies = Policy.Admin, Roles = Roles.Admin)]
public record DeleteApplicationServiceCommand(int ApplicationID, int ApplicationServiceID)
    : IAuthorizeableRequest<ErrorOr<int>>
{
}

public class DeleteApplicationServiceCommandValidator :
    AbstractValidator<DeleteApplicationServiceCommand>
{
    public DeleteApplicationServiceCommandValidator()
    {
        RuleFor(x => x.ApplicationID).ApplicationID();
        RuleFor(x => x.ApplicationServiceID).ApplicationServiceID();
    }
}