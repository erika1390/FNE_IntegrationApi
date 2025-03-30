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

            builder.Property(m => m.Code).IsRequired().HasMaxLength(50);
            builder.Property(m => m.Name).IsRequired().HasMaxLength(100);
            builder.Property(m => m.Route).HasMaxLength(255);
            builder.Property(m => m.Icon).HasMaxLength(100);
            builder.Property(m => m.Order).IsRequired();
        }
    }
}