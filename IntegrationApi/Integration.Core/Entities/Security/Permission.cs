using Integration.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Integration.Core.Entities.Security
{
    [Table("Permissions", Schema = "Security")]
    public class Permission : BaseEntity
    {
        [Required, MaxLength(10)]
        public required string Code { get; set; }
        [Required, MaxLength(50)]
        public string? Name { get; set; }
        public virtual ICollection<RoleModulePermissions> RoleModulePermissions { get; set; } = new HashSet<RoleModulePermissions>();
    }
}