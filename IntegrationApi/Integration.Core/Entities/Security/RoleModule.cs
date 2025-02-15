using Integration.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Integration.Core.Entities.Security
{
    [Table("RoleModules", Schema = "Security")]
    public class RoleModule : BaseEntity
    {
        [Key]
        public int RoleModuleId { get; set; }
        [ForeignKey("Role")]
        public int RoleId { get; set; } 
        [ForeignKey("Module")]
        public int ModuleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual Module Module { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}