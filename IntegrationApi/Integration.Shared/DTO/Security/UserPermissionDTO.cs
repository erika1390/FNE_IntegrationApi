namespace Integration.Shared.DTO.Security
{
    public class UserPermissionDTO
    {
        public required string CodeUser { get; set; }
        public required string UserName { get; set; }
        public required string CodeRole { get; set; }
        public required string Role { get; set; }
        public required string CodeModule { get; set; }
        public required string Module { get; set; }
        public required string CodePermission { get; set; }
        public required string Permission { get; set; }
    }
}