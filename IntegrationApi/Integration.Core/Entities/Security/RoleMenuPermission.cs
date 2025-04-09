using Integration.Core.Entities.Base;
using Integration.Core.Interfaces.Identity;

using System.ComponentModel.DataAnnotations.Schema;

namespace Integration.Core.Entities.Security
{
    public class RoleMenuPermission : BaseEntity, IAuditableEntity
    {
        public int RoleId { get; set; }
        public int  MenuId { get; set; }
        public int PermissionId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        [ForeignKey("MenuId")]
        public virtual Menu Menu { get; set; }

        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }
    }
}