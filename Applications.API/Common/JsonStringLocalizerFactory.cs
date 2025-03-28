using Applications.Usecase.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Globalization;

namespace Applications.API.Common;

public class LocalizationMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var cultureKey = context.Request.Headers[nameof(IUserContextProvider.Language)];
        if (string.IsNullOrEmpty(cultureKey))
            cultureKey = "fa";

        if (DoesCultureExist(cultureKey))
        {
            var culture = new System.Globalization.CultureInfo(cultureKey);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
        await next(context);
    }
    private static bool DoesCultureExist(string cultureName)
    {
        return CultureInfo.GetCultures(CultureTypes.AllCultures).Any(culture => string.Equals(culture.Name, cultureName,
StringComparison.CurrentCultureIgnoreCase));
    }
}

public class JsonStringLocalizerFactory(
    IDistributedCache cache
    ) : IStringLocalizerFactory
{
    public IStringLocalizer Create(Type resourceSource)
    {
        return new JsonStringLocalizer(cache);
    }
    public IStringLocalizer Create(string baseName, string location)
    {
        return new JsonStringLocalizer(cache);
    }
}

public class JsonStringLocalizer(
    IDistributedCache cache
    ) : IStringLocalizer
{
    private readonly JsonSerializer _serializer = new();

    public LocalizedString this[string name]
    {
        get
        {
            var value = GetString(name);
            return new LocalizedString(name, value);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var actualValue = this[name];
            return !actualValue.ResourceNotFound
                ? new LocalizedString(name, string.Format(actualValue.Value, arguments))
                : actualValue;
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var filePath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";

        using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using StreamReader streamReader = new(stream);
        using JsonTextReader reader = new(streamReader);

        while (reader.Read())
        {
            if (reader.TokenType != JsonToken.PropertyName)
                continue;

            var key = reader.Value as string;
            reader.Read();
            var value = _serializer.Deserialize<string>(reader);
            yield return new LocalizedString(key, value);
        }
    }

    private string GetString(string key)
    {
        var filePath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";
        var fullFilePath = Path.GetFullPath(filePath);
        if (!File.Exists(fullFilePath))
            return string.Empty;

        var cacheKey = $"locale_{Thread.CurrentThread.CurrentCulture.Name}_{key}";
        var cacheValue = cache.GetString(cacheKey);
        if (!string.IsNullOrEmpty(cacheValue))
            return cacheValue;

        var result = GetValueFromJSON(key, fullFilePath);
        if (!string.IsNullOrEmpty(result))
            cache.SetString(cacheKey, result);

        return result;
    }

    private string GetValueFromJSON(string propertyName, string filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(filePath))
                return string.Empty;

            using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader streamReader = new(stream);
            using JsonTextReader reader = new(streamReader);

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName && reader.Value as string == propertyName)
                {
                    reader.Read();
                    return _serializer.Deserialize<string>(reader);
                }
            }

            return string.Empty;
        }
        catch (JsonReaderException ex)
        {
            //todo: Log or debug information about the exception.
            Console.WriteLine($"Error reading JSON at line {ex.LineNumber}, position {ex.LinePosition}: {ex.Message}");
            return string.Empty;
        }
    }
}
