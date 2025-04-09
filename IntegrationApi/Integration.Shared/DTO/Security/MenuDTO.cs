namespace Integration.Shared.DTO.Security
{
    public class MenuDTO
    {
        public string? ParentMenuCode { get; set; }
        public required string ModuleCode { get; set; }
        public string Code { get; set; }
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
        public ICollection<MenuDTO> SubMenus { get; set; }
        public ModuleDTO Module { get; set; } = null!;
    }
}
