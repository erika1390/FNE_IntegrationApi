using Integration.Core.Entities.Security;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Integration.Infrastructure.Data.Configurations.Security
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<Integration.Core.Entities.Security.Application>
    {
        public void Configure(EntityTypeBuilder<Integration.Core.Entities.Security.Application> builder)
        {
            builder.ToTable("Applications", "Security");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(e => e.Code)
                .IsUnique();

            builder.HasIndex(e => e.Name)
                .IsUnique();

            builder.HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey("ApplicationId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Modules)
                .WithOne()
                .HasForeignKey("ApplicationId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}