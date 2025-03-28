using Applications.Usecase.Common.Security;
using ErrorOr;
using FluentValidation;
using MediatR;

namespace Applications.Usecase.Application.Commands;

[Authorize(Permissions = Permissions.Application.Delete, Policies = Policy.Admin, Roles = Roles.Admin)]
public record DeleteApplicationCommand(int ID)
    : IRequest<ErrorOr<int>>
{
}

public class DeleteApplicationCommandValidator :
    AbstractValidator<DeleteApplicationCommand>
{
    public DeleteApplicationCommandValidator()
    {
        RuleFor(x => x.ID)
            .GreaterThan(0)
            .WithMessage(Resources.ResourceKey.IdInvalid);
    }
}