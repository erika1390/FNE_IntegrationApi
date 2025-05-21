using Integration.Core.Entities.Base;

using System.ComponentModel.DataAnnotations;

namespace Integration.Core.Entities.Parametric
{
    public class City : BaseEntity
    {
        public int DepartmentId { get; set; }
        [Required, MaxLength(5)]
        public string CodeDane { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }

        public virtual Department Department { get; set; } = null!;
    }
}