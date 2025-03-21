using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace Integration.Shared.DTO.Header
{
    public class HeaderDTO
    {
        [FromHeader(Name = "ApplicationCode")]
        [Required(ErrorMessage = "El campo ApplicationCode es obligatorio.")]
        public string ApplicationCode { get; set; }

        [FromHeader(Name = "RoleCode")]
        [Required(ErrorMessage = "El campo RolCode es obligatorio.")]
        public string RoleCode { get; set; }
        
        [FromHeader(Name = "UserCode")]
        [Required(ErrorMessage = "El campo UserCode es obligatorio.")]
        public string UserCode { get; set; }

        [FromHeader(Name = "Authorization")]
        [Required(ErrorMessage = "El campo Authorization es obligatorio.")]
        public string Authorization { get; set; }
    }
}