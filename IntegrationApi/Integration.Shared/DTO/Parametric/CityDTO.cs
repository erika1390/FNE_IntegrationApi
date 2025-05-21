namespace Integration.Shared.DTO.Parametric
{
    public class CityDTO
    {
        public int CityId { get; set; }
        public int DepartmentId { get; set; }
        public string CodeDane { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public required string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}