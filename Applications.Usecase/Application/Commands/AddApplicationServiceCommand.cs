using Applications.Usecase.Application;
using Applications.Usecase.Application.Dto;
using FluentValidation;

namespace Applications.Usecase.Application.Commands;

[Authorize(Permissions = Application.Permissions.Application.Update, Policies = Policy.Admin, Roles = Roles.Admin)]
public record AddApplicationServiceCommand(
    int ApplicationID,
    ApplicationServiceInputDTO ApplicationService)
    : IAuthorizeableRequest<ErrorOr<int>>
{
}

public class AddApplicationServiceCommandValidator
    : AbstractValidator<AddApplicationServiceCommand>
{
    public AddApplicationServiceCommandValidator()
    {
        RuleFor(x => x.ApplicationID).ApplicationID();
        RuleFor(x => x.ApplicationService).NotNull().
            SetValidator(new ApplicationServiceValidator());
    }
}