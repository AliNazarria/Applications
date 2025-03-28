using Applications.Usecase.Common.Security;
using ErrorOr;
using FluentValidation;
using MediatR;

namespace Applications.Usecase.Service.Commands;

[Authorize(Permissions = Permissions.Service.Delete, Policies = Policy.Admin, Roles = Roles.Admin)]
public record DeleteServiceCommand(int ID)
    : IRequest<ErrorOr<int>>
{
}

public class DeleteApplicationCommandValidator :
    AbstractValidator<DeleteServiceCommand>
{
    public DeleteApplicationCommandValidator()
    {
        RuleFor(x => x.ID)
            .GreaterThan(0)
            .WithMessage(Resources.ResourceKey.IdInvalid);
    }
}