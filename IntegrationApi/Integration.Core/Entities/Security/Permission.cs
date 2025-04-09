using Integration.Core.Entities.Base;
using Integration.Core.Interfaces.Identity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Integration.Core.Entities.Security
{
    [Table("Permissions", Schema = "Security")]
    public class Permission : BaseEntity, IAuditableEntity
    {
        [Required, MaxLength(10)]
        public required string Code { get; set; }
        [Required, MaxLength(50)]
        public string? Name { get; set; }
    }
}