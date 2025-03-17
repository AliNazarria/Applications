using Applications.API.Util;
using Applications.Usecase.Common.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Applications.API.Common;

public class CheckRequiredHeaderParameter() : IMiddleware
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
                context.Response.WriteAsJsonAsync(ResponseHelper.ToError(400, $"{item.Name} is required"));
            }
            if (!info.AllowEmptyValue
                && string.IsNullOrWhiteSpace(context.Request.Headers[item.Name]))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized; ;
                context.Response.WriteAsJsonAsync(ResponseHelper.ToError(400, $"{item.Name} is empty"));
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