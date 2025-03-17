using FluentMigrator;

namespace Applications.Migration;

[Migration(202503141950, "Add Table Service")]
public class _202503141950_AddTableService : FluentMigrator.Migration
{
    public override void Up()
    {
        IfDatabase("SqlServer", "Postgres").
        Create.Table("Service")
            .WithColumn("ID").AsInt32().Identity().PrimaryKey()
            .WithColumn("Key").AsString(150).NotNullable()
            .WithColumn("Name").AsString(150).NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Deleted").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("Created_By").AsInt32().Nullable()
            .WithColumn("Created_At").AsInt32().Nullable()
            .WithColumn("Updated_By").AsInt32().Nullable()
            .WithColumn("Updated_At").AsInt32().Nullable();

        Insert.IntoTable("Service").Row(new { Key = "SMS", Name = "پیامک", Active = true, Deleted = false });
    }
    public override void Down()
    {
        IfDatabase("SqlServer", "Postgres").
        Delete.Table("Service");
    }
}