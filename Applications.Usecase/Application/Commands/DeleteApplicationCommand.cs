using FluentValidation;

namespace Applications.Usecase.Application.Commands;


[Authorize(Permissions = Permissions.Application.Delete, Policies = Policy.Admin, Roles = Roles.Admin)]
public record DeleteApplicationCommand(int ID)
    : IAuthorizeableRequest<ErrorOr<int>>
{
}

public class DeleteApplicationCommandValidator :
    AbstractValidator<DeleteApplicationCommand>
{
    public DeleteApplicationCommandValidator()
    {
        RuleFor(x => x.ID).ApplicationID();
    }
}