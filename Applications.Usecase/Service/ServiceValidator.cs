using FluentValidation;
using System.Net;

namespace Applications.Usecase.Service;

public static class ServiceValidator
{
    public static IRuleBuilderOptions<T, string>
    ServiceName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotNull().NotEmpty()
            .WithMessage(Resources.NameInvalid)
            .WithErrorCode(HttpStatusCode.BadRequest.ToString());
    }
    public static IRuleBuilderOptions<T, int>
    ServiceId<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .NotNull().GreaterThan(0)
            .WithMessage(Resources.IdInvalid)
            .WithErrorCode(HttpStatusCode.BadRequest.ToString());
    }
}
