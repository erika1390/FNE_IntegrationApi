namespace Integration.Shared.DTO.Security
{
    public class UserRoleDTO
    {
        public required string UserCode { get; set; }
        public required string RoleCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public required string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public UserDTO? User { get; set; }
        public RoleDTO? Role { get; set; }
    }
}