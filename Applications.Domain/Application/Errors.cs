using ErrorOr;

namespace Applications.Domain.Application;

public static class Errors
{
    public static Error ApplicationServiceNotFound() => Error.Validation(
            code: nameof(Application.Services),
            description: $"ApplicationServiceNotFound");

    public static Error ApplicationServiceIsDuplicate() => Error.Validation(
            code: nameof(Application.Services),
            description: $"ApplicationServiceIsDuplicate");
}
