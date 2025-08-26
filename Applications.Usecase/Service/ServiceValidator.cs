using Applications.Usecase.Service.Dto;
using FluentValidation;
using System.Net;

namespace Applications.Usecase.Service;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, int>
        ServiceID<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .NotNull().GreaterThan(0)
            .WithMessage(Resources.IdInvalid)
            .WithErrorCode(HttpStatusCode.BadRequest.ToString());
    }
}

public class ServiceValidation
    : AbstractValidator<ServiceInputDTO>
{
    public ServiceValidation()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty()
            .WithMessage(Resources.NameInvalid)
            .WithErrorCode(HttpStatusCode.BadRequest.ToString());
    }
}