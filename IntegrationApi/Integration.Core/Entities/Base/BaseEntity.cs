using System.ComponentModel.DataAnnotations;
namespace Integration.Core.Entities.Base
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [Required]
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        [MaxLength(50)]
        public string? UpdatedBy { get; set; }

        public bool IsActive { get; set; } = true;
    }
}