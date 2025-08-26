using FluentValidation;

namespace Applications.Usecase.Application.Queries;

//[Authorize(Permissions = Permissions.Application.Get, Policies = Policy.Guest, Roles = "")]
public record GetAllAplicationQuery()
    : IRequest<ErrorOr<List<ApplicationDTO>>>
{
}

public class GetAllAplicationQueryValidator
    : AbstractValidator<GetAllAplicationQuery>
{
    public GetAllAplicationQueryValidator()
    {
    }
}