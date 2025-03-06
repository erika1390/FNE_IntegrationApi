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
        [Required, MaxLength(255)]
        public string? Name { get; set; }
        public virtual ICollection<RoleModule> RoleModules { get; set; } = new HashSet<RoleModule>();
    }
}