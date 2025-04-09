using Integration.Core.Entities.Base;
using Integration.Core.Interfaces.Identity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Integration.Core.Entities.Security
{
    [Table("Applications", Schema = "Security")]
    public class Application : BaseEntity, IAuditableEntity
    {
        [Required, MaxLength(10)]
        public required string Code { get; set; }
        [Required, MaxLength(50)]
        public required string Name { get; set; }
        public virtual ICollection<Role>? Roles { get; set; }
        public virtual ICollection<Module>? Modules { get; set; }
    }
}