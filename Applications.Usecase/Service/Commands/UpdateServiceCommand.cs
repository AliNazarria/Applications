using FluentValidation;

namespace Applications.Usecase.Service.Commands;

[Authorize(Permissions = Permissions.Service.Set, Policies = Policy.Admin, Roles = Roles.Admin)]
public record UpdateServiceCommand(
    int ID,
    string Key,
    string Name,
    bool Active) : IAuthorizeableRequest<ErrorOr<int>>
{
}

public class UpdateServiceCommandValidator
    : AbstractValidator<UpdateServiceCommand>
{
    public UpdateServiceCommandValidator()
    {
        RuleFor(x => x.ID).ServiceId();
        RuleFor(x => x.Name).ServiceName();
    }
}