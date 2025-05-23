namespace Applications.Usecase.Service;

public static class ServiceErrors
{
    public static Error ServiceNotFound(string desc = "") => Error.Validation(
        code: Resources.ServiceNotFound,
        description: desc);
    public static Error ServiceDeletedFailed(string desc = "") => Error.Validation(
        code: Resources.ServiceDeleteFailed,
        description: desc);
    public static Error ServiceSetFailed(string desc = "") => Error.Validation(
        code: Resources.ServiceSetFailed,
        description: desc);
}
