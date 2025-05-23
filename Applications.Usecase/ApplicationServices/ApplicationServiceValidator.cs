using FluentValidation;
using System.Net;

namespace Applications.Usecase.ApplicationServices;

public static class ApplicationServiceValidator
{
    public static IRuleBuilderOptions<T, int>
    ApplicationServiceId<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .NotNull().GreaterThan(0)
            .WithMessage(Resources.IdInvalid)
            .WithErrorCode(HttpStatusCode.BadRequest.ToString());
    }
}