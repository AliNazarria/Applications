using ErrorOr;

namespace Applications.Usecase.Common.Interfaces;

public interface ILoggerServiceProvider
{
    Task<ErrorOr<Success>> LogUserActivity();
}