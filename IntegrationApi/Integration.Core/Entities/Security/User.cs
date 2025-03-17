using Integration.Core.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Integration.Core.Entities.Security
{
    [Table("Users", Schema = "Security")]
    public class User : IdentityUser<int>, IAuditableEntity
    {
        [Required, MaxLength(10)]
        public required string Code { get; set; }
        [Required, MaxLength(100)]
        public required string FirstName { get; set; }
        [Required, MaxLength(100)]
        public required string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        [MaxLength(50)]
        public required string CreatedBy { get; set; } = "System";
        [MaxLength(50)]
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual ICollection<UserRole>? UserRoles { get; set; }
    }
}