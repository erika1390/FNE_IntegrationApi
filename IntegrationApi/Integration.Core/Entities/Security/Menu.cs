using Integration.Core.Entities.Base;
using Integration.Core.Interfaces.Identity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Integration.Core.Entities.Security
{
    [Table("Menu", Schema = "Security")]
    public class Menu : BaseEntity, IAuditableEntity
    {
        public int? ParentMenuId { get; set; }
        public int ModuleId { get; set; }
        [Required, MaxLength(10)]
        public required string Code { get; set; }
        [Required, MaxLength(50)]
        public required string Name { get; set; }
        public string? Route { get; set; }
        public string? Icon { get; set; }
        public int Order { get; set; }

        public virtual Menu? ParentMenu { get; set; }
        public virtual ICollection<Menu> SubMenus { get; set; } = new List<Menu>();
        public virtual Module Module { get; set; } = null!;
    }
}