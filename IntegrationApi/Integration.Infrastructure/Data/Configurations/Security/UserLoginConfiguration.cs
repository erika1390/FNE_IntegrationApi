using Integration.Core.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Integration.Infrastructure.Data.Configurations.Security
{
    public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable("UserLogins", "Security");
            builder.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            builder.HasIndex(ul => ul.UserId)
                .HasDatabaseName("IDX_UserLogins_UserId");

            builder.Property(e => e.LoginProvider)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.ProviderKey)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.ProviderDisplayName)
                .HasMaxLength(255);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}