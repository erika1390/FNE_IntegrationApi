namespace Integration.Shared.DTO.Audit
{
    public class LogDTO
    {
        public required string CodeApplication { get; set; }
        public required string CodeUser { get; set; }
        public required string UserIp { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Level { get; set; } = "Information";
        public string Message { get; set; } = string.Empty;
        public string? Exception { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string? Request { get; set; }
        public string? Response { get; set; }
        public long? DurationMs { get; set; }
    }
}