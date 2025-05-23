using FluentValidation;
using System.Net;

namespace Applications.Usecase;

public static class Extentions
{
    public static IRuleBuilderOptions<T, int>
    PageSize<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .LessThanOrEqualTo(Constants.MaxPageSize)
            .WithMessage(Resources.PageSizeInvalid)
            .WithErrorCode(HttpStatusCode.BadRequest.ToString());
    }
}