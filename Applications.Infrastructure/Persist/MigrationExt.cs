using Applications.Migration;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Applications.Infrastructure.Persist;

public static class MigrationExt
{
    public static void MigrateUp(string connectionString)
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(AddTableApplication).Assembly).For.Migrations())
            .BuildServiceProvider(false);

        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}
