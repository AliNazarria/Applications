using Applications.Usecase.Application.Dto;
using FluentValidation;

namespace Applications.Usecase.Application.Queries;

[Authorize(Permissions = Permissions.Application.Report, Policies = Policy.Guest, Roles = "")]
public record GetAllApplicationServiceQuery()
    : IAuthorizeableRequest<ErrorOr<List<ApplicationServiceDTO>>>
{
}

public class GetAllApplicationServiceQueryValidator
    : AbstractValidator<GetAllApplicationServiceQuery>
{
    public GetAllApplicationServiceQueryValidator()
    {
    }
}