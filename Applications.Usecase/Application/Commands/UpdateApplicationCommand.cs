using Applications.Usecase.Application.Dto;
using FluentValidation;

namespace Applications.Usecase.Application.Commands;

[Authorize(Permissions = Permissions.Application.Update, Policies = Policy.Admin, Roles = Roles.Admin)]
public record UpdateApplicationCommand(int ID, ApplicationInputDTO Application)
    : IAuthorizeableRequest<ErrorOr<int>>
{
}

public class UpdateApplicationCommandValidator
    : AbstractValidator<UpdateApplicationCommand>
{
    public UpdateApplicationCommandValidator()
    {
        RuleFor(x => x.ID).ApplicationID();
        RuleFor(x => x.Application).NotNull()
            .SetValidator(new ApplicationValidator());
    }
}