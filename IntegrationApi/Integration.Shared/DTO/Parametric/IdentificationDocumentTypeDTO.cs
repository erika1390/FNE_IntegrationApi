namespace Integration.Shared.DTO.Parametric
{
    public class IdentificationDocumentTypeDTO
    {
        public int Id { get; set; }
        public required string Abbreviation { get; set; }
        public required string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public required string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}