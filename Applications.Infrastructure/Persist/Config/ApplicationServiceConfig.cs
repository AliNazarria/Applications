using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Applications.Infrastructure.Persist.Config;

public class ApplicationServiceConfig : IEntityTypeConfiguration<Common.Domain.Entities.ApplicationService>
{
    public void Configure(EntityTypeBuilder<Common.Domain.Entities.ApplicationService> builder)
    {
        builder.ToTable($"{nameof(Common.Domain.Entities.ApplicationService)}", schema: DataSchemaConstants.DEFAULT_SCHEMA_NAME);
        builder.Property(p => p.ID).IsRequired()
          .UseIdentityColumn().ValueGeneratedOnAdd();

        builder.HasKey(x => x.ID);
        builder.Property(x => x.Deleted);
        builder.Property(x => x.Active);
        builder.Property(x => x.Created_At);
        builder.Property(x => x.Created_By);
        builder.Property(x => x.Updated_At);
        builder.Property(x => x.Updated_By);

        builder.Property(x => x.ApplicationID);
        builder.Property(x => x.ServiceID);

        //builder.Navigation(x => x.Application);//.AutoInclude();
        //builder.HasOne(x => x.Application)
        //    .WithMany(x => x.Services)
        //    .HasForeignKey(x => x.ApplicationID);
    }
}