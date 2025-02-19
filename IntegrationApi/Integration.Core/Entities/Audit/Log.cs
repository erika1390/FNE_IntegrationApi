using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Integration.Core.Entities.Audit
{
    [Table("Logs", Schema = "Audit")]
    public class Log
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(10)]
        public required string CodeApplication { get; set; }

        [Required, MaxLength(10)]
        public required string CodeUser { get; set; }

        [Required, MaxLength(100)]
        public required string UserIp { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [Required, MaxLength(20)]
        public string Level { get; set; } = "Information";

        [Required, Column(TypeName = "nvarchar(max)")]
        public string Message { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(max)")]
        public string? Exception { get; set; }

        [Required, MaxLength(100)]
        public string Source { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Method { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(max)")]
        public string? Request { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Response { get; set; }

        public long? DurationMs { get; set; }
    }
}