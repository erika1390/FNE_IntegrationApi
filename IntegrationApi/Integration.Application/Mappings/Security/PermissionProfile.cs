using AutoMapper;
using Integration.Core.Entities.Security;
using Integration.Shared.DTO.Security;
namespace Integration.Application.Mappings.Security
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            CreateMap<PermissionDTO, Permission>()
                .ForMember(dest => dest.PermissionId, opt => opt.MapFrom(src => src.PermissionId))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.RolePermissions, opt => opt.Ignore());

            CreateMap<Permission, PermissionDTO>()
                .ForMember(dest => dest.PermissionId, opt => opt.MapFrom(src => src.PermissionId))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}