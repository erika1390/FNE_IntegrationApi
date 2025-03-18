using AutoMapper;
using Integration.Core.Entities.Security;
using Integration.Shared.DTO.Security;
namespace Integration.Application.Mappings.Security
{
    public class RoleModulePermissionProfile : Profile
    {
        public RoleModulePermissionProfile()
        {
            CreateMap<Role, RoleDTO>();
            CreateMap<Module, ModuleDTO>();
            CreateMap<Permission, PermissionDTO>();
            CreateMap<RoleModulePermissions, RoleModulePermissionDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.Module, opt => opt.MapFrom(src => src.Module))
                .ForMember(dest => dest.Permission, opt => opt.MapFrom(src => src.Permission))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ReverseMap();
        }
    }
}