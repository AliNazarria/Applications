using ErrorOr;

namespace Applications.Domain.Application;

public static class Errors
{
    public static Error ApplicationServiceNotFound(string desc = "") => Error.Validation(
            code: Resources.ApplicationServiceNotFound,
            description: desc);

    public static Error ApplicationServiceIsDuplicate(string desc = "") => Error.Validation(
            code: Resources.ApplicationServiceIsDuplicate,
            description: desc);
}