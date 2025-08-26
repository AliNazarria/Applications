namespace Applications.Usecase.Application;

public static class ApplicationErrors
{
    public static Error ApplicationNotFound(string desc = "") => Error.Validation(
        code: Resources.ApplicationNotFound,
        description: desc);
    public static Error ApplicationDeletedFailed(string desc = "") => Error.Validation(
        code: Resources.ApplicationDeleteFailed,
        description: desc);
    public static Error ApplicationSetFailed(string desc = "") => Error.Validation(
        code: Resources.ApplicationSetFailed,
        description: desc);

    public static Error ApplicationServiceSetFailed(string desc = "") => Error.Validation(
        code: Resources.ApplicationServiceSetFailed,
        description: desc);

    public static Error ApplicationServiceDeleteFailed(string desc = "") => Error.Validation(
        code: Resources.ApplicationServiceDeleteFailed,
        description: desc);
}
