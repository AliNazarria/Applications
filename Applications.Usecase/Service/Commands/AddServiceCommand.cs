using FluentValidation;

namespace Applications.Usecase.Service.Commands;

[Authorize(Permissions = Permissions.Service.Set, Policies = Policy.Admin, Roles = Roles.Admin)]
public record AddServiceCommand(
    string Key,
    string Name,
    bool Active) : IAuthorizeableRequest<ErrorOr<int>>
{
}

public class AddServiceCommandValidator
    : AbstractValidator<AddServiceCommand>
{
    public AddServiceCommandValidator()
    {
        RuleFor(x => x.Name).ServiceName();
    }
}