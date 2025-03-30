using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Integration.Core.Entities.Security;

namespace Integration.Infrastructure.Data.Configurations.Security
{
    public class RoleMenuPermissionConfiguration : IEntityTypeConfiguration<RoleMenuPermission>
    {
        public void Configure(EntityTypeBuilder<RoleMenuPermission> builder)
        {
            builder.ToTable("RoleMenuPermission", "Security");

            builder.HasKey(rmp => rmp.Id);

            builder.HasOne(rmp => rmp.Role)
                   .WithMany()
                   .HasForeignKey(rmp => rmp.RoleId);

            builder.HasOne(rmp => rmp.Menu)
                   .WithMany()
                   .HasForeignKey(rmp => rmp.MenuId);

            builder.HasOne(rmp => rmp.Permission)
                   .WithMany()
                   .HasForeignKey(rmp => rmp.PermissionId);
        }
    }
}