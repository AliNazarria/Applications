using FluentMigrator;

namespace Applications.Migration;

[Migration(202502282012, "Add Table Application")]
public class AddTableApplication : FluentMigrator.Migration
{
    public override void Up()
    {
        IfDatabase("SqlServer", "Postgres").
        Create.Table("Application")
            .WithColumn("ID").AsInt32().Identity().PrimaryKey()
            .WithColumn("Key").AsString(150).NotNullable()
            .WithColumn("Title").AsString(150).NotNullable()
            .WithColumn("Comment").AsString(int.MaxValue).Nullable()
            .WithColumn("LogoAddress").AsString(int.MaxValue).Nullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Deleted").AsBoolean().NotNullable().WithDefaultValue(false);
    }
    public override void Down()
    {
        IfDatabase("SqlServer", "Postgres").
        Delete.Table("Application");
    }
}