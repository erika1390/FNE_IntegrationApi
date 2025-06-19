namespace Integration.Shared.DTO.Security
{
    public class UserRoleAppDTO
    {
        public int UserId { get; set; }
        public string UserCode { get; set; }
        public string FullName { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string AppCode { get; set; }
        public string AppName { get; set; }
    }
}