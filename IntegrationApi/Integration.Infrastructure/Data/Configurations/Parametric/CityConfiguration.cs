using Integration.Core.Entities.Parametric;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Integration.Infrastructure.Data.Configurations.Parametric
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("City", "Parametric");
            builder.HasKey(e => e.Id);
            builder.Property(m => m.CodeDane)
                .IsRequired()
                .HasMaxLength(5);
            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(50);
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
        }
    }
}