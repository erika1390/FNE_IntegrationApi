using AutoMapper;

using Integration.Core.Entities.Security;
using Integration.Shared.DTO.Security;
namespace Integration.Application.Mappings.Security
{
    public class ModuleProfile : Profile
    {
        public ModuleProfile()
        {
            CreateMap<ModuleDTO, Module>()
                .ForMember(dest => dest.ModuleId, opt => opt.MapFrom(src => src.ModuleId))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.ApplicationId))
                .ForMember(dest => dest.Application, opt => opt.Ignore())
                .ForMember(dest => dest.RoleModules, opt => opt.Ignore());

            CreateMap<Module, ModuleDTO>()
                    .ForMember(dest => dest.ModuleId, opt => opt.MapFrom(src => src.ModuleId))
                    .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.ApplicationId));
        }
    }
}