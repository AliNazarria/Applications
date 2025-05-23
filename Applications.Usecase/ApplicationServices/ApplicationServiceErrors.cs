namespace Applications.Usecase.ApplicationServices;

public static class ApplicationServiceErrors
{
    public static Error ApplicationServiceSetFailed(string desc = "") => Error.Validation(
        code: Resources.ApplicationServiceSetFailed,
        description: desc);

    public static Error ApplicationServiceDeleteFailed(string desc = "") => Error.Validation(
        code: Resources.ApplicationServiceDeleteFailed,
        description: desc);
}