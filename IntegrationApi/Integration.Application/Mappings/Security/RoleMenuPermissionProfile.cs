using AutoMapper;
using Integration.Core.Entities.Security;
using Integration.Shared.DTO.Security;
namespace Integration.Application.Mappings.Security
{
    public class RoleMenuPermissionProfile : Profile
    {
        public RoleMenuPermissionProfile()
        {
            CreateMap<Role, RoleDTO>();
            CreateMap<Module, ModuleDTO>();
            CreateMap<Permission, PermissionDTO>();
            CreateMap<RoleMenuPermission, RoleMenuPermissionDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.Menu, opt => opt.MapFrom(src => src.Menu))
                .ForMember(dest => dest.Permission, opt => opt.MapFrom(src => src.Permission))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ReverseMap();
        }
    }
}