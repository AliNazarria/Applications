using FluentMigrator;

namespace Applications.Migration;

[Migration(202503061821, "Alter Table Application")]
public class _202503061821_AlterTableApplication : FluentMigrator.Migration
{
    public override void Up()
    {
        Alter.Table("Application").AddColumn("Created_By").AsInt32().Nullable();
        Alter.Table("Application").AddColumn("Created_At").AsInt32().Nullable();
        Alter.Table("Application").AddColumn("Updated_By").AsInt32().Nullable();
        Alter.Table("Application").AddColumn("Updated_At").AsInt32().Nullable();
    }

    public override void Down()
    {
        Delete
            .Column("Created_By").Column("Created_At")
            .Column("Updated_By").Column("Updated_At")
            .FromTable("Application");
    }
}