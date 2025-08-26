using Applications.Usecase.Service.Dto;
using FluentValidation;

namespace Applications.Usecase.Service.Commands;

[Authorize(Permissions = Permissions.Service.Set, Policies = Policy.Admin, Roles = Roles.Admin)]
public record AddServiceCommand(ServiceInputDTO Service)
    : IAuthorizeableRequest<ErrorOr<int>>
{
}

public class AddServiceCommandValidator
    : AbstractValidator<AddServiceCommand>
{
    public AddServiceCommandValidator()
    {
        RuleFor(x => x.Service).NotNull().SetValidator(new ServiceValidation());
    }
}