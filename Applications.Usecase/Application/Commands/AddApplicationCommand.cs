using FluentValidation;

namespace Applications.Usecase.Application.Commands;

[Authorize(Permissions = Permissions.Application.Update, Policies = Policy.Admin, Roles = Roles.Admin)]
public record AddApplicationCommand(
    string Key,
    string Title,
    string Comment,
    string LogoAddress,
    bool Active) : IAuthorizeableRequest<ErrorOr<int>>
{
}

public class AddApplicationCommandValidator
    : AbstractValidator<AddApplicationCommand>
{
    public AddApplicationCommandValidator()
    {
        RuleFor(x => x.Title).Title();
    }
}