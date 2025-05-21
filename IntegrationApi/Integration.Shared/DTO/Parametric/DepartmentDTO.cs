namespace Integration.Shared.DTO.Parametric
{
    public class DepartmentDTO
    {
        public int Id { get; set; }
        public required string CodeDane { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public required string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}