using System.ComponentModel.DataAnnotations;

namespace Integration.Shared.DTO.Security
{
    public class MenuDTO
    {
        public int? ParentMenuId { get; set; }
        public int ModuleId { get; set; }
        public required string CodeMenu { get; set; }
        [Required, MaxLength(50)]
        public required string Name { get; set; }
        public string? Route { get; set; }
        public string? Icon { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public required string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public MenuDTO? ParentMenu { get; set; }
    }
}
