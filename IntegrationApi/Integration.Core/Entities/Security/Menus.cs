using Integration.Core.Entities.Base;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Integration.Core.Entities.Security
{
    [Table("Menus", Schema = "Security")]
    public class Menus : BaseEntity
    {
        [ForeignKey("Module")]
        public int ModuleId { get; set; }
        public int? ParentMenuId { get; set; }
        [Required, MaxLength(10)]
        public required string Code { get; set; }
        [Required, MaxLength(50)]
        public required string Name { get; set; }
        public string? Route { get; set; }
        public string? Icon { get; set; }
        public int Order { get; set; }

        // Relaciones
        public Module Module { get; set; } = null!;
        public ICollection<Menus> SubMenus { get; set; } = new List<Menus>();
        public ICollection<RoleMenuPermission> RoleModuleMenuPermissions { get; set; } = new List<RoleMenuPermission>();
    }
}