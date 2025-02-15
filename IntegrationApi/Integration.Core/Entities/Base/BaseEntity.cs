using System.ComponentModel.DataAnnotations;

namespace Integration.Core.Entities.Base
{
    public abstract class BaseEntity
    {
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [Required]
        [MaxLength(50)]
        public string CreatedBy { get; set; } = "System"; // Usuario que creó el registro

        [MaxLength(50)]
        public string UpdatedBy { get; set; } // Usuario que modificó el registro

        public bool IsActive { get; set; } = true;
    }
}