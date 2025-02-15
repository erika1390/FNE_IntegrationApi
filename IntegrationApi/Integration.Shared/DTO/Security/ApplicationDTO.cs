namespace Integration.Shared.DTO.Security
{
    public class ApplicationDTO
    {
        public int ApplicationId { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; }
        public required string CreatedBy { get; set; }
        public required string UpdatedBy { get; set; } 
        public bool IsActive { get; set; }
    }
}
