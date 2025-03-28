using Applications.Usecase.Common.Security;
using ErrorOr;
using FluentValidation;
using MediatR;

namespace Applications.Usecase.ApplicationServices.Commands;

[Authorize(Permissions = Application.Permissions.Application.Delete, Policies = Policy.Admin, Roles = Roles.Admin)]
public record DeleteApplicationServiceCommand(int ID)
    : IRequest<ErrorOr<int>>
{
}

public class DeleteApplicationServiceCommandValidator :
    AbstractValidator<DeleteApplicationServiceCommand>
{
    public DeleteApplicationServiceCommandValidator()
    {
        RuleFor(x => x.ID)
            .GreaterThan(0)
            .WithMessage(Resources.ResourceKey.IdInvalid);
    }
}