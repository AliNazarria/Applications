using Applications.Domain.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Applications.Infrastructure.Persist.Config;

public class ServiceConfig : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable($"{nameof(Service)}", schema: DataSchemaConstants.DEFAULT_SCHEMA_NAME);
        builder.Property(p => p.ID).IsRequired()
          .UseIdentityColumn().ValueGeneratedOnAdd();

        builder.HasKey(x => x.ID);
        builder.Property(x => x.Deleted);
        builder.Property(x => x.Active);
        builder.Property(x => x.Created_At);
        builder.Property(x => x.Created_By);
        builder.Property(x => x.Updated_At);
        builder.Property(x => x.Updated_By);

        builder.ComplexProperty(p => p.Name).Property(p => p.Value)
            .HasColumnName("Name").HasMaxLength(DataSchemaConstants.MAX_TITLE_LENGTH);

        builder.ComplexProperty(p => p.Key).Property(p => p.Value)
            .HasColumnName("Key").HasMaxLength(DataSchemaConstants.MAX_TITLE_LENGTH);
    }
}