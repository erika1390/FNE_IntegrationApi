using System.ComponentModel.DataAnnotations;

namespace Integration.Shared.DTO.Security
{
    public class LoginRequestDTO
    {
        public required string ApplicationCode { get; set; }
        public required string RoleCode { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}