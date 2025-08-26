using Applications.Usecase.Application.Dto;
using FluentValidation;

namespace Applications.Usecase.Application.Commands;

[Authorize(Permissions = Permissions.Application.Update, Policies = Policy.Admin, Roles = Roles.Admin)]
public record AddApplicationCommand(ApplicationInputDTO Application)
    : IAuthorizeableRequest<ErrorOr<int>>
{
}

public class AddApplicationCommandValidator
    : AbstractValidator<AddApplicationCommand>
{
    public AddApplicationCommandValidator()
    {
        RuleFor(x => x.Application).NotNull()
            .SetValidator(new ApplicationValidator());
    }
}