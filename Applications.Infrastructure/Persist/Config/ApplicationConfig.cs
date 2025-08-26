using Applications.Domain.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Applications.Infrastructure.Persist.Config;

public class ApplicationConfig : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.ToTable($"{nameof(Application)}", schema: DataSchemaConstants.DEFAULT_SCHEMA_NAME);
        builder.Property(p => p.ID).IsRequired()
          .UseIdentityColumn().ValueGeneratedOnAdd();

        builder.HasKey(x => x.ID);
        builder.HasIndex(x => x.ID);
        builder.Property(x => x.Deleted);
        builder.Property(x => x.Active);
        builder.Property(x => x.Created_At);
        builder.Property(x => x.Created_By);
        builder.Property(x => x.Updated_At);
        builder.Property(x => x.Updated_By);

        builder.ComplexProperty(p => p.LogoAddress).Property(p => p.Value)
            .HasColumnName("LogoAddress");

        builder.ComplexProperty(p => p.Description).Property(p => p.Value)
            .HasColumnName("Description").HasMaxLength(DataSchemaConstants.MAX_COMMENT_LENGTH);

        builder.ComplexProperty(p => p.Title).Property(p => p.Value)
            .HasColumnName("Title").HasMaxLength(DataSchemaConstants.MAX_TITLE_LENGTH);

        builder.ComplexProperty(p => p.Key).Property(p => p.Value)
            .HasColumnName("Key").HasMaxLength(DataSchemaConstants.MAX_TITLE_LENGTH);

        builder.Navigation(x => x.Services);
        builder
            .HasMany(e => e.Services)
            .WithOne(e => e.Application)
            .HasForeignKey(e => e.ApplicationID)
            .HasPrincipalKey(e => e.ID)
            .IsRequired(false);
    }
}