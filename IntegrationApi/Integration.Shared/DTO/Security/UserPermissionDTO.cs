using System.Security.Cryptography.X509Certificates;

namespace Integration.Shared.DTO.Security
{
    public class UserPermissionDTO
    {
        public string CodeUser { get; set; }
        public string UserName { get; set; }
        public List<RoleDto> Roles { get; set; }
    }

    public class RoleDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<ModuleDto> Modules { get; set; }
    }

    public class ModuleDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<MenuDto> Menus { get; set; }
    }

    public class MenuDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<PermissionDto> Permissions { get; set; }
    }

    public class PermissionDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}