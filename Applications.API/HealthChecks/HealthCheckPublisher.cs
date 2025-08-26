using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog.Context;

namespace Applications.API.HealthChecks;

public class HealthCheckPublisher(
    ILogger<HealthCheckPublisher> logger
    ) : IHealthCheckPublisher
{
    public Task PublishAsync(HealthReport report,
        CancellationToken cancellationToken)
    {
        using (LogContext.PushProperty("IsHealthCheck", true))
        {
            logger.LogInformation("HealthCheck status: {@HealthReport}", report);
        }
        return Task.CompletedTask;
    }
}