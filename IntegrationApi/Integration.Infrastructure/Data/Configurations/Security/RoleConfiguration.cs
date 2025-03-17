using Integration.Core.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Integration.Infrastructure.Data.Configurations.Security
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles", "Security");
            builder.HasKey(e => e.Id);

            builder.HasIndex(r => r.Code)
                .HasDatabaseName("IDX_Roles_Code")
                .IsUnique();

            builder.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(e => e.Code)
                .IsUnique();

            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.Property(e => e.UpdatedAt);

            builder.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.UpdatedBy)
                .HasMaxLength(50);

            builder.Property(e => e.IsActive)
                .IsRequired();

            builder.HasOne(e => e.Application)
                .WithMany(a => a.Roles)
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.RoleModulePermissions)
                .WithOne(rm => rm.Role)
                .HasForeignKey(rm => rm.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}