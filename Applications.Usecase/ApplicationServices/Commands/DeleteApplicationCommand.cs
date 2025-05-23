using Common.Usecase.Security;
using FluentValidation;

namespace Applications.Usecase.ApplicationServices.Commands;

[Authorize(Permissions = Application.Permissions.Application.Update, Policies = Policy.Admin, Roles = Roles.Admin)]
public record DeleteApplicationServiceCommand(int ID)
    : IAuthorizeableRequest<ErrorOr<int>>
{
}

public class DeleteApplicationServiceCommandValidator :
    AbstractValidator<DeleteApplicationServiceCommand>
{
    public DeleteApplicationServiceCommandValidator()
    {
        RuleFor(x => x.ID).ApplicationServiceId();
    }
}