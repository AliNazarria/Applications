using FluentValidation;

namespace Applications.Usecase.Service.Commands;

[Authorize(Permissions = Permissions.Service.Delete, Policies = Policy.Admin, Roles = Roles.Admin)]
public record DeleteServiceCommand(int ID)
    : IAuthorizeableRequest<ErrorOr<int>>
{
}

public class DeleteApplicationCommandValidator :
    AbstractValidator<DeleteServiceCommand>
{
    public DeleteApplicationCommandValidator()
    {
        RuleFor(x => x.ID).ServiceId();
    }
}