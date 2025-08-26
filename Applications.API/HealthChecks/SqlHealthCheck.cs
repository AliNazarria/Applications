using Applications.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace Applications.API.HealthChecks;

public class SqlHealthCheck(
    IOptions<ConnectionStringsConfigOptions> options
    ) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var sqlConnection = new SqlConnection(options.Value.Database);

            await sqlConnection.OpenAsync(cancellationToken);

            using var command = sqlConnection.CreateCommand();
            command.CommandText = "SELECT 1";

            await command.ExecuteScalarAsync(cancellationToken);

            return HealthCheckResult.Healthy(description: $"{command.CommandText} was executed succesfully");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                context.Registration.FailureStatus.ToString(),
                exception: ex);
        }
    }
}