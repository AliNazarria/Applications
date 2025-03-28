using Applications.API.Util;
using Applications.Usecase.Common.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Applications.API.Common;

public class CheckRequiredHeaderParameter(IResponseHelper responseHelper)
    : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        PropertyInfo[] userContextParameter = typeof(IUserContextProvider).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var item in userContextParameter)
        {
            var info = item.GetCustomAttribute<HeaderParameterAttribute>();
            if (info.Required
                && !context.Request.Headers.ContainsKey(item.Name))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return context.Response.WriteAsJsonAsync(responseHelper.ErrorResult(ErrorOr.Error.Unauthorized(code: nameof(item), description: $"{item.Name}IsRequired")));
            }


            if (!info.AllowEmptyValue)
            {
                int headerParameter;
                var headerParameterValue = context.Request.Headers[item.Name];
                bool isNumeric = int.TryParse(headerParameterValue, out headerParameter);
                if ((isNumeric && headerParameter < 1)
                    || string.IsNullOrWhiteSpace(headerParameterValue))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return context.Response.WriteAsJsonAsync(responseHelper.ErrorResult(ErrorOr.Error.Unauthorized(code: nameof(item), description: $"{item.Name}Invalid")));
                }
            }
        }

        return next.Invoke(context);
    }
}
public class AddRequiredHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!context.ApiDescription.RelativePath.ToLower().StartsWith(ApiResources.ApiBasePath.ToLower()))
            return;

        operation.Parameters = operation.Parameters ?? new List<OpenApiParameter>();
        PropertyInfo[] userContextParameter = typeof(IUserContextProvider).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var item in userContextParameter)
        {
            var info = item.GetCustomAttribute<HeaderParameterAttribute>();
            operation.Parameters.Add(new OpenApiParameter
            {
                In = ParameterLocation.Header,
                Name = item.Name,
                AllowEmptyValue = info?.AllowEmptyValue ?? false,
                Required = info?.Required ?? true,
                Description = info?.Description ?? "",
            });
        }
    }
}