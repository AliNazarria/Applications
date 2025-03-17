using Applications.Usecase.Common.Interfaces;
using Applications.Usecase.Common.Security;
using Applications.Usecase.Service.Interfaces;
using ErrorOr;
using FluentValidation;

namespace Applications.Usecase.ApplicationServices.Commands;

[Authorize(Permissions = Application.Permissions.Application.Update, Policies = Policy.Admin, Roles = Roles.Admin)]
public record AddApplicationServiceCommand(
    int application,
    int service,
    bool active) : IAuthorizeableRequest<ErrorOr<int>>
{
}

public class AddApplicationServiceCommandValidator
    : AbstractValidator<AddApplicationServiceCommand>
{
    public AddApplicationServiceCommandValidator(
        IResourceLocalizer localizer,
        IServiceRepository serviceRepository,
        IDateTimeProvider dateTimeProvider)
    {
        RuleFor(x => x.application)
            .NotNull().NotEmpty().GreaterThan(0)
            .WithMessage(localizer.Localize(Resources.ResourceKey.IdInvalid));
        RuleFor(x => x.service)
            .NotNull().NotEmpty().GreaterThan(0)
            .WithMessage(localizer.Localize(Resources.ResourceKey.IdInvalid));
    }
}