using Integration.Core.Entities.Security;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Integration.Infrastructure.Data.Configurations.Security
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable("RoleClaims", "Security");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.RoleId)
                .IsRequired();

            builder.Property(e => e.ClaimType)
                .HasMaxLength(255);

            builder.Property(e => e.ClaimValue)
                .HasMaxLength(1000);

            builder.HasOne<Role>()
                .WithMany()
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
