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

            // Clave primaria compuesta en lugar de Id
            builder.HasKey(e => new { e.RoleId, e.ModuleId, e.PermissionId });

            // Índice único para optimización
            builder.HasIndex(rm => new { rm.RoleId, rm.ModuleId, rm.PermissionId })
                .HasDatabaseName("IDX_RoleModules_RoleId_ModuleId_PermissionId")
                .IsUnique();

            // Relaciones y restricciones de eliminación
            builder.HasOne(e => e.Role)
                .WithMany(r => r.RoleModules)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Module)
                .WithMany(m => m.RoleModules)
                .HasForeignKey(e => e.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Permission)
                .WithMany(p => p.RoleModules)
                .HasForeignKey(e => e.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}