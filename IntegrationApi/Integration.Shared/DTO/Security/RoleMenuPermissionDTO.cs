namespace Integration.Shared.DTO.Security
{
    public class RoleMenuPermissionDTO
    {
        public required string RoleCode { get; set; }
        public string MenuCode { get; set; }
        public string PermissionCode { get; set; }
        public RoleDTO? Role { get; set; }
        public MenuDTO? Menu { get; set; }
        public PermissionDTO? Permission { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public required string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}