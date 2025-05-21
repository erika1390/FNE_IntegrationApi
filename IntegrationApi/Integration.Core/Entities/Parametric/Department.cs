using Integration.Core.Entities.Base;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Integration.Core.Entities.Parametric
{
    [Table("Department", Schema = "Parametric")]
    public class Department : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(5)]
        public string CodeDane { get; set;}
        [Required, MaxLength(50)]
        public string Name { get; set; }
    }
}