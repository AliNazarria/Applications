using Applications.Usecase.Service.Dto;
using FluentValidation;

namespace Applications.Usecase.Service.Queries;

[Authorize(Permissions = Permissions.Service.Get, Policies = Policy.Guest, Roles = "")]
public record GetServiceQuery(int ID)
    : IAuthorizeableRequest<ErrorOr<ServiceDTO>>
{
}

public class GetServiceQueryValidator
    : AbstractValidator<GetServiceQuery>
{
    public GetServiceQueryValidator()
    {
        RuleFor(x => x.ID).ServiceID();
    }
}