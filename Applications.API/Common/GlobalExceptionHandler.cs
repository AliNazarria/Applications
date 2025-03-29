using Microsoft.AspNetCore.Diagnostics;

namespace Applications.API.Common;

internal sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger
    )
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = ResponseDTO.Exception(exception);
        httpContext.Response.StatusCode = problemDetails.Status;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}