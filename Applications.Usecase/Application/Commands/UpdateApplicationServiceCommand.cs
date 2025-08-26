using Applications.Usecase.Application;
using Applications.Usecase.Application.Dto;
using FluentValidation;

namespace Applications.Usecase.Application.Commands;

[Authorize(Permissions = Application.Permissions.Application.Update, Policies = Policy.Admin, Roles = Roles.Admin)]
public record UpdateApplicationServiceCommand(
    int ApplicationID,
    int ApplicationServiceID, 
    ApplicationServiceInputDTO ApplicationService)
    : IAuthorizeableRequest<ErrorOr<int>>
{
}

public class UpdateApplicationServiceCommandValidator :
    AbstractValidator<UpdateApplicationServiceCommand>
{
    public UpdateApplicationServiceCommandValidator()
    {
        RuleFor(x => x.ApplicationID).ApplicationID();
        RuleFor(x => x.ApplicationServiceID).ApplicationServiceID();
        RuleFor(x => x.ApplicationService).NotNull()
            .SetValidator(new ApplicationServiceValidator());
    }
}