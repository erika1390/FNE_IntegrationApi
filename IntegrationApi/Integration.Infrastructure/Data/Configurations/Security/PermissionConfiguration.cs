using Integration.Core.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Integration.Infrastructure.Data.Configurations.Security
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions", "Security");
            builder.HasKey(e => e.Id);

            builder.HasIndex(p => p.Code)
                .HasDatabaseName("IDX_Permissions_Code")
                .IsUnique();

            builder.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(e => e.Name)
                .HasMaxLength(255);

            builder.HasIndex(e => e.Code)
                .IsUnique();
        }
    }
}