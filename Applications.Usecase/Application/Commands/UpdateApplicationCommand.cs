using Applications.Usecase.Application.Interfaces;
using Applications.Usecase.Common.Interfaces;
using Applications.Usecase.Common.Security;
using ErrorOr;
using FluentValidation;

namespace Applications.Usecase.Application.Commands;

[Authorize(Permissions = Permissions.Application.Update, Policies = Policy.Admin, Roles = Roles.Admin)]
public record UpdateApplicationCommand(
    int ID,
    string Key,
    string Title,
    string Comment,
    string LogoAddress,
    bool Active
    ) : IAuthorizeableRequest<ErrorOr<int>>
{
}

public class UpdateApplicationCommandValidator
    : AbstractValidator<UpdateApplicationCommand>
{
    public UpdateApplicationCommandValidator(
        IApplicationRepository applicationRepository
        )
    {
        RuleFor(x => x.ID)
            .GreaterThan(0)
            .WithMessage(Resources.ResourceKey.IdInvalid);

        RuleFor(x => x.Title)
            .NotNull().NotEmpty()
            .WithMessage(Resources.ResourceKey.TitleInvalid);

        //RuleFor(x => x.Key)
        //    .NotNull().NotEmpty()
        //    .WithMessage(Resources.ResourceKey.KeyInvalid);
        //RuleFor(x => x).MustAsync(async (command, token) =>
        //{
        //    return await applicationRepository.IsUnique(command.ID, command.Key);
        //}).WithMessage(Resources.ResourceKey.KeyIsDuplicated);
    }
}