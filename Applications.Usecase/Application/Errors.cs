namespace Applications.Usecase.Application;

public static class Errors
{
    public static Error ApplicationNotFound() => Error.Validation(
        code: nameof(Domain.Application.Application),
        description: $"ApplicationNotFound");
    public static Error ApplicationDeletedFailed() => Error.Validation(
        code: nameof(Domain.Application.Application),
        description: $"ApplicationDeletedFailed");
    public static Error ApplicationSetFailed() => Error.Validation(
        code: nameof(Domain.Application.Application),
        description: $"ApplicationSetFailed");
}
