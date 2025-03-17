using FluentMigrator;

namespace Applications.Migration;

[Migration(202503151634, "Add Table Application Service")]
public class _202503151634_AddTableApplicationService : FluentMigrator.Migration
{
    public override void Up()
    {
        IfDatabase("SqlServer", "Postgres").
        Create.Table("ApplicationService")
            .WithColumn("ID").AsInt32().Identity().PrimaryKey()
            .WithColumn("ApplicationID").AsInt32().NotNullable()
                .ForeignKey("fk_Application_AppService", "Application", "ID")
            .WithColumn("ServiceID").AsInt32().NotNullable()
                .ForeignKey("fk_Service_AppService", "Service", "ID")
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Deleted").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("Created_By").AsInt32().Nullable()
            .WithColumn("Created_At").AsInt32().Nullable()
            .WithColumn("Updated_By").AsInt32().Nullable()
            .WithColumn("Updated_At").AsInt32().Nullable();
    }
    public override void Down()
    {
        if (Schema.Table("ApplicationService").Constraint("fk_Application_AppService").Exists())
        {
            Delete.ForeignKey("fk_Application_AppService").OnTable("ApplicationService");
        }
        if (Schema.Table("ApplicationService").Constraint("fk_Service_AppService").Exists())
        {
            Delete.ForeignKey("fk_Service_AppService").OnTable("ApplicationService");
        }
        Delete.Table("ApplicationService");
    }
}