using Applications.Usecase.Application.Interfaces;
using Applications.Usecase.Common.Interfaces;
using Applications.Usecase.Common.Security;
using ErrorOr;
using FluentValidation;
using System.Net;

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
    public AddApplicationCommandValidator(
        IApplicationRepository applicationRepository
        )
    {
        RuleFor(x => x.Title)
            .NotNull().NotEmpty()
            .WithMessage(Resources.ResourceKey.Application.TitleInvalid)
            .WithErrorCode(HttpStatusCode.BadRequest.ToString());

        //RuleFor(x => x.Key)
        //    .NotNull().NotEmpty()
        //    .WithMessage(Resources.ResourceKey.KeyInvalid)
        //    .WithErrorCode(HttpStatusCode.BadRequest.ToString());

        //RuleFor(x => x).MustAsync(async (command, token) =>
        //{
        //    return await applicationRepository.IsUnique(0, command.Key);
        //}).WithMessage(Resources.ResourceKey.KeyIsDuplicated)
        //.WithErrorCode(HttpStatusCode.BadRequest.ToString());
    }
}
