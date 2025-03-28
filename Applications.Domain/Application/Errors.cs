using ErrorOr;

namespace Applications.Domain.Application;

public static class Errors
{
    public static Error ApplicationServiceNotFound { get; } = Error.Validation(
            code: nameof(Application.Services),
            description: $"ApplicationServiceNotFound");

    public static Error ApplicationServiceIsDuplicate { get; } = Error.Validation(
            code: nameof(Application.Services),
            description: $"ApplicationServiceIsDuplicate");
}
