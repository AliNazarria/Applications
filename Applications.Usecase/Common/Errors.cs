using ErrorOr;

namespace Applications.Usecase.Common;

public static class Errors
{
    public static Error FilterInvalid(string code = "") => Error.Validation(
        code: code,
        description: $"FilterInvalid");
}