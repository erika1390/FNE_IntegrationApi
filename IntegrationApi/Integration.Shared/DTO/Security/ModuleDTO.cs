namespace Integration.Shared.DTO.Security
{
    public class ModuleDTO
    {
        public string Code { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public required string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public ApplicationDTO? Application { get; set; }
    }
}