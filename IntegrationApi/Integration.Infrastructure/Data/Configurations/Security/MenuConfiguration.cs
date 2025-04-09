using Integration.Core.Entities.Security;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Integration.Infrastructure.Data.Configurations.Security
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable("Menu", "Security");

            builder.HasKey(m => m.Id);

            builder.HasIndex(m => m.Code)
               .HasDatabaseName("IDX_Menus_Code")
               .IsUnique();

            builder.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.Route)
                .HasMaxLength(100);

            builder.Property(m => m.Icon)
                .HasMaxLength(50);

            builder.Property(m => m.Order)
                .IsRequired();
        }
    }
}