using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
namespace Integration.Core.Entities.Security
{
    public class UserRole : IdentityUserRole<int>
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public required string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; } = true;

        // Relaciones correctas sin ForeignKey redundantes
        public virtual User? User { get; set; }
        public virtual Role? Role { get; set; }
    }
}