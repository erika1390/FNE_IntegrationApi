namespace Integration.Shared.DTO.Security
{
    public class LoginRequestDTO
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}