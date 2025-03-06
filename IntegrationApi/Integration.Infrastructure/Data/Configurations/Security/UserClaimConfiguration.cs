using Integration.Core.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Integration.Infrastructure.Data.Configurations.Security
{
    public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable("UserClaims", "Security");
            builder.HasKey(e => e.Id);

            builder.HasIndex(uc => uc.UserId)
                .HasDatabaseName("IDX_UserClaims_UserId");

            builder.Property(e => e.UserId)
                .IsRequired();

            builder.Property(e => e.ClaimType)
                .HasMaxLength(255);

            builder.Property(e => e.ClaimValue)
                .HasMaxLength(1000);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}