using Integration.Core.Entities.Base;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Integration.Core.Entities.Security
{
    [Table("Permissions", Schema = "Security")]
    public class Permission : BaseEntity
    {
        [Key]
        public int PermissionId { get; set; }
        [Required, MaxLength(255)]
        public string Name { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}