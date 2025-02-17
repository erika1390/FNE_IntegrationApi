using System.ComponentModel.DataAnnotations;

namespace Integration.Shared.DTO.Aut
{
    public class LoginRequestDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}