using Integration.Core.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Integration.Infrastructure.Data.Configurations.Security
{
    public class RoleModuleConfiguration : IEntityTypeConfiguration<RoleModule>
    {
        public void Configure(EntityTypeBuilder<RoleModule> builder)
        {
            builder.ToTable("RoleModules", "Security");
            builder.HasKey(e => e.RoleModuleId);

            builder.Property(e => e.RoleModuleId)
                .IsRequired();

            builder.HasOne(e => e.Role)
                .WithMany(r => r.RoleModules)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Module)
                .WithMany(m => m.RoleModules)
                .HasForeignKey(e => e.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.RolePermissions)
                .WithOne(rp => rp.RoleModule)
                .HasForeignKey(rp => rp.RoleModuleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}