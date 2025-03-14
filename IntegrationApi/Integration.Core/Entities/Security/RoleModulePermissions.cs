using Integration.Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
namespace Integration.Core.Entities.Security
{
    [Table("RoleModulePermissions", Schema = "Security")]
    public class RoleModulePermissions : BaseEntity
    {
        [ForeignKey("Role")]
        public int RoleId { get; set; } 
        [ForeignKey("Module")]
        public int ModuleId { get; set; }
        [ForeignKey("Permission")]
        public int PermissionId { get; set; }
        public virtual Role? Role { get; set; }
        public virtual Module? Module { get; set; }
        public virtual Permission? Permission { get; set; }
    }
}