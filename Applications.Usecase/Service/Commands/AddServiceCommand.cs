using Applications.Usecase.Common.Interfaces;
using Applications.Usecase.Common.Security;
using Applications.Usecase.Service.Interfaces;
using ErrorOr;
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
    public AddServiceCommandValidator(
        IServiceRepository serviceRepository,
        IDateTimeProvider dateTimeProvider)
    {
        RuleFor(x => x.Key)
            .NotNull().NotEmpty()
            .WithMessage(Resources.ResourceKey.KeyInvalid);
        RuleFor(x => x.Name)
            .NotNull().NotEmpty()
            .WithMessage(Resources.ResourceKey.KeyInvalid);
        RuleFor(x => x).MustAsync(async (command, token) =>
        {
            return await serviceRepository.IsUnique(0, command.Key);
        }).WithMessage(Resources.ResourceKey.KeyIsDuplicated);
    }
}
