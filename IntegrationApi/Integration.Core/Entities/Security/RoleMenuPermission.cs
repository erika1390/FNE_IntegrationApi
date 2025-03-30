using Integration.Core.Entities.Base;

namespace Integration.Core.Entities.Security
{
    public class RoleMenuPermission : BaseEntity
    {
        public int RoleId { get; set; }
        public int  MenuId { get; set; }
        public int PermissionId { get; set; }
        public Role Role { get; set; }
        public Menus Menu { get; set; }
        public Permission Permission { get; set; }
    }
}