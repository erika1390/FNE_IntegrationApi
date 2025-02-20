namespace Integration.Shared.DTO.Security
{
    public class ModuleDTO
    {
        public int ModuleId { get; set; }
        public required string Code { get; set; }
        public required string Name { get; set; }
        public int ApplicationId { get; set; }
    }
}