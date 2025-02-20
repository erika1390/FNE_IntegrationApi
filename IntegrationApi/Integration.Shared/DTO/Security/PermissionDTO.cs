namespace Integration.Shared.DTO.Security
{
    public class PermissionDTO
    {
        public int PermissionId { get; set; }
        public required string Code { get; set; }
        public string? Name { get; set; }
    }
}