using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Integration.Core.Entities.Base;
namespace Integration.Core.Entities.Security
{
    [Table("Modules", Schema = "Security")]
    public class Module : BaseEntity
    {
        [Required, MaxLength(10)]
        public required string Code { get; set; }
        [Required, MaxLength(50)]
        public required string Name { get; set; }
        [ForeignKey("Application")]
        public int ApplicationId { get; set; }
        public virtual Application? Application { get; set; }
        public virtual ICollection<RoleModulePermissions>? RoleModulePermissions { get; set; }
    }
}