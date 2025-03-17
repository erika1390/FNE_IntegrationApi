using Microsoft.AspNetCore.Mvc;

namespace Integration.Shared.DTO.Header
{
    public class HeaderDTO
    {
        [FromHeader(Name = "ApplicationCode")]
        public string ApplicationCode { get; set; }

        [FromHeader(Name = "UserCode")]
        public string UserCode { get; set; }
    }
}