using Applications.Usecase.Common.Security;
using ErrorOr;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Usecase.ApplicationServices.Commands;

[Authorize(Permissions = Application.Permissions.Application.Delete, Policies = Policy.Admin, Roles = Roles.Admin)]
public record DeleteApplicationServiceCommand(int ID)
    : IRequest<ErrorOr<int>>
{
}

public class DeleteApplicationServiceCommandValidator :
    AbstractValidator<DeleteApplicationServiceCommand>
{
    public DeleteApplicationServiceCommandValidator(IResourceLocalizer localizer)
    {
        RuleFor(x => x.ID)
            .GreaterThan(0)
            .WithMessage(localizer.Localize(Resources.ResourceKey.IdInvalid));
    }
}