using Integration.Core.Entities.Audit;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Integration.Infrastructure.Data.Configurations.Audit
{
    public class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("Logs", "Audit");
            builder.HasKey(e => e.LogId);

            builder.Property(e => e.CodeApplication)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(e => e.CodeUser)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(e => e.UserIp)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Timestamp)
                .IsRequired();

            builder.Property(e => e.Level)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Information");

            builder.Property(e => e.Message)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(e => e.Exception)
                .HasColumnType("nvarchar(max)");

            builder.Property(e => e.Source)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Method)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Request)
                .HasColumnType("nvarchar(max)");

            builder.Property(e => e.Response)
                .HasColumnType("nvarchar(max)");

            builder.Property(e => e.DurationMs);
        }
    }
}