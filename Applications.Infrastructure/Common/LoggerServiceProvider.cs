using Applications.Usecase.Common.Interfaces;
using ErrorOr;

namespace Applications.Infrastructure.Common;

public class LoggerServiceProvider 
    : ILoggerServiceProvider
{
    public async Task<ErrorOr<Success>> LogUserActivity()
    {
        //todo => log
        return Result.Success;
    }
}