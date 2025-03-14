using Integration.Core.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Integration.Infrastructure.Data.Configurations.Security
{
    public class RoleModulePermissionConfiguration : IEntityTypeConfiguration<RoleModulePermissions>
    {
        public void Configure(EntityTypeBuilder<RoleModulePermissions> builder)
        {
            builder.ToTable("RoleModulePermissions", "Security");

            // Clave primaria compuesta en lugar de Id
            builder.HasKey(e => new { e.RoleId, e.ModuleId, e.PermissionId });

            // Índice único para optimización
            builder.HasIndex(rm => new { rm.RoleId, rm.ModuleId, rm.PermissionId })
                .HasDatabaseName("IDX_RoleModules_RoleId_ModuleId_PermissionId")
                .IsUnique();

            // Relaciones y restricciones de eliminación
            builder.HasOne(e => e.Role)
                .WithMany(r => r.RoleModulePermissions)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Module)
                .WithMany(m => m.RoleModulePermissions)
                .HasForeignKey(e => e.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Permission)
                .WithMany(p => p.RoleModulePermissions)
                .HasForeignKey(e => e.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}