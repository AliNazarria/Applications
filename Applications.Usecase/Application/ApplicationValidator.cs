using FluentValidation;
using System.Net;

namespace Applications.Usecase.Application;

public static class ApplicationValidator
{
    public static IRuleBuilderOptions<T, string>
    Title<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotNull().NotEmpty()
            .WithMessage(Resources.TitleInvalid)
            .WithErrorCode(HttpStatusCode.BadRequest.ToString());
    }
    public static IRuleBuilderOptions<T, int>
    ApplicationId<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .NotNull().GreaterThan(0)
            .WithMessage(Resources.IdInvalid)
            .WithErrorCode(HttpStatusCode.BadRequest.ToString());
    }
}