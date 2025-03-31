namespace Applications.Usecase.Service;

public static class Errors
{
    public static Error ServiceNotFound() => Error.Validation(
        code: nameof(Domain.Application.Application),
        description: $"ServiceNotFound");
    public static Error ServiceDeletedFailed() => Error.Validation(
        code: nameof(Domain.Application.Application),
        description: $"ServiceDeletedFailed");
    public static Error ServiceSetFailed() => Error.Validation(
        code: nameof(Domain.Application.Application),
        description: $"ServiceSetFailed");
}
