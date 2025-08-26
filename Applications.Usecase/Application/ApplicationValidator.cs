using Applications.Usecase.Application.Dto;
using Applications.Usecase.Service;
using FluentValidation;
using System.Net;

namespace Applications.Usecase.Application;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, int>
        ApplicationID<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .NotNull().GreaterThan(0)
            .WithMessage(Resources.IdInvalid)
            .WithErrorCode(HttpStatusCode.BadRequest.ToString());
    }
    public static IRuleBuilderOptions<T, int>
        ApplicationServiceID<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .NotNull().GreaterThan(0)
            .WithMessage(Resources.IdInvalid)
            .WithErrorCode(HttpStatusCode.BadRequest.ToString());
    }
}

public class ApplicationValidator
    : AbstractValidator<ApplicationInputDTO>
{
    public ApplicationValidator()
    {
        RuleFor(x => x.Title).NotNull().NotEmpty()
            .WithMessage(Resources.TitleInvalid)
            .WithErrorCode(HttpStatusCode.BadRequest.ToString());

    }
}
public class ApplicationServiceValidator :
    AbstractValidator<ApplicationServiceInputDTO>
{
    public ApplicationServiceValidator()
    {
        RuleFor(x => x.ServiceID).ServiceID();
    }
}