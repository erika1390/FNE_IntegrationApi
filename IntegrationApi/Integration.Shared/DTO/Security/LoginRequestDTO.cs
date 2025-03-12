namespace Integration.Shared.DTO.Security
{
    public class LoginRequestDTO
    {
        public string ApplicationCode { get; set; }
        public string RoleCode { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}