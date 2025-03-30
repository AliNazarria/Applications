using Applications.Usecase.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace Applications.Infrastructure.Common;

public sealed class UserContextProvider(IHttpContextAccessor httpContextAccessor)
    : IUserContextProvider
{
    public int AppID => GetHeader<int>(nameof(AppID));
    public int ServiceID => GetHeader<int>(nameof(ServiceID));
    public Guid UserID => GetHeader<Guid>(nameof(UserID));
    public string Language => GetHeader<string>(nameof(Language));
    public string CorrelationId => GetHeader<string>(nameof(CorrelationId));

    private T GetHeader<T>(string headerKey)
    {
        var toReturn = default(T);

        Microsoft.Extensions.Primitives.StringValues headerValues;
        if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue(headerKey, out headerValues))
        {
            var valueString = headerValues.FirstOrDefault();
            if (valueString != null)
            {
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(valueString);
            }
        }

        return toReturn;
    }
}