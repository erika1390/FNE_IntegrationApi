using Integration.Core.Entities.Security;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Integration.Infrastructure.Data.Configurations.Security
{
    public class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.ToTable("Modules", "Security");
            builder.HasKey(e => e.Id);

            builder.HasIndex(m => m.Code)
                .HasDatabaseName("IDX_Modules_Code")
                .IsUnique();

            builder.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(e => e.Code)
                .IsUnique();

            builder.HasOne(e => e.Application)
                .WithMany(a => a.Modules)
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.RoleModules)
                .WithOne(rm => rm.Module)
                .HasForeignKey(rm => rm.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}