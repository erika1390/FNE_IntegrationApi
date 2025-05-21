using Integration.Core.Entities.Base;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Integration.Core.Entities.Parametric
{
    [Table("IdentificationDocumentType", Schema = "Parametric")]
    public class IdentificationDocumentType : BaseEntity
    {
        [Required, MaxLength(5)]
        public required string Abbreviation { get; set; }

        [Required, MaxLength(50)]
        public required string Description { get; set; }
    }
}