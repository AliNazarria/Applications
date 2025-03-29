using ErrorOr;

namespace Applications.Usecase.ApplicationServices;

public static class Errors
{
    public static Error ApplicationServiceSetFailed() => Error.Validation(
        code: nameof(Domain.Application.ApplicationService),
        description: $"ApplicationServiceSetFailed");

    public static Error ApplicationServiceDeleteFailed() => Error.Validation(
        code: nameof(Domain.Application.ApplicationService),
        description: $"ApplicationServiceDeletedFailed");
}