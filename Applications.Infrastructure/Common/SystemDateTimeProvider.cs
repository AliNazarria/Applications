using Applications.Usecase.Common.Interfaces;

namespace Applications.Infrastructure.Common;

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime Max() => DateTime.MaxValue;
    public DateTime Min() => DateTime.MinValue;
    public DateTime Now() => DateTime.Now;
    public DateTime UtcNow() => DateTime.UtcNow;
    public int NowTimeStampInSecound()
    {
        return (int)(Now() - new DateTime(1970, 1, 1)).TotalSeconds;
    }
    public DateTime Get(int timestamp)
    {
        return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
    }
}
