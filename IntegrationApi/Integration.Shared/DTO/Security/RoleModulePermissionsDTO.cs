namespace Integration.Shared.DTO.Security
{
    public class RoleModulePermissionsDTO
    {
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
        public int PermissionId { get; set; }
        public RoleDTO? Role { get; set; }
        public ModuleDTO? Module { get; set; }
        public PermissionDTO? Permission { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public required string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}