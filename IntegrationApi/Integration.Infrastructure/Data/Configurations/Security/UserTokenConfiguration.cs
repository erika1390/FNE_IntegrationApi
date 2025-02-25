using Integration.Core.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Integration.Infrastructure.Data.Configurations.Security
{
    public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable("UserTokens", "Security");
            builder.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            builder.Property(e => e.LoginProvider)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.Value)
                .HasColumnType("nvarchar(max)");

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}