﻿using Integration.Core.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Integration.Core.Entities.Security
{
    [Table("Roles", Schema = "Security")]
    public class Role : IdentityRole<int>, IAuditableEntity
    {
        [Required, MaxLength(255)]
        public string Name { get; set; }
        [ForeignKey("Application")]
        public int ApplicationId { get; set; }        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public required string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual Application Application { get; set; }
        public virtual ICollection<RoleModule> RoleModules { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    }
}