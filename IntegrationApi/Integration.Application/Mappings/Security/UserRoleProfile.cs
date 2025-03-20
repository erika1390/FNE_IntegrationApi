using AutoMapper;
using Integration.Core.Entities.Security;
using Integration.Shared.DTO.Security;

namespace Integration.Application.Mappings.Security
{
    public class UserRoleProfile : Profile
    {
        public UserRoleProfile()
        {
            CreateMap<User, UserDTO>(); 
            CreateMap<Role, RoleDTO>();
            CreateMap<UserRoleDTO, UserRole>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ReverseMap();
        }
    }
}