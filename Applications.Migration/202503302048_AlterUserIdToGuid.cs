using FluentMigrator;

namespace Applications.Migration;

[Migration(202503302048, "Alter All Tables And Change UserInfo to Guid")]
public class _202503302048_AlterUserIdToGuid : FluentMigrator.Migration
{
    public override void Up()
    {
        Delete.Column("Created_By").Column("Updated_By").FromTable("Application");
        Alter.Table("Application").AddColumn("Created_By").AsGuid().Nullable();
        Alter.Table("Application").AddColumn("Updated_By").AsGuid().Nullable();

        Delete.Column("Created_By").Column("Updated_By").FromTable("Service");
        Alter.Table("Service").AddColumn("Created_By").AsGuid().Nullable();
        Alter.Table("Service").AddColumn("Updated_By").AsGuid().Nullable();

        Delete.Column("Created_By").Column("Updated_By").FromTable("ApplicationService");
        Alter.Table("ApplicationService").AddColumn("Created_By").AsGuid().Nullable();
        Alter.Table("ApplicationService").AddColumn("Updated_By").AsGuid().Nullable();
    }

    public override void Down()
    {
        
    }
}