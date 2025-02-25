using Integration.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Integration.Core.Entities.Security
{
    [Table("RolePermissions", Schema = "Security")]
    public class RolePermission : BaseEntity
    {
        [Key]
        public int RolePermissionId { get; set; }

        public int RoleId { get; set; }
        public virtual Role? Role { get; set; }

        public int PermissionId { get; set; }

        [ForeignKey(nameof(PermissionId))] // 🔹 Se fuerza la asociación
        public virtual Permission? Permission { get; set; }

        public int RoleModuleId { get; set; }
        public virtual RoleModule? RoleModule { get; set; }
    }
}
