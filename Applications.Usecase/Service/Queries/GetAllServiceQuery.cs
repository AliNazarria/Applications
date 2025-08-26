using Applications.Usecase.Service.Dto;
using FluentValidation;

namespace Applications.Usecase.Service.Queries;

[Authorize(Permissions = Permissions.Service.Get, Policies = Policy.Guest, Roles = "")]
public record GetAllServiceQuery()
    : IAuthorizeableRequest<ErrorOr<List<ServiceDTO>>>
{
}

public class GetAllServiceQueryValidator
    : AbstractValidator<GetAllServiceQuery>
{
    public GetAllServiceQueryValidator()
    {
    }
}