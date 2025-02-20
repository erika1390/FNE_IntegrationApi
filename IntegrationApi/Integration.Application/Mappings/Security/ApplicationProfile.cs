using AutoMapper;
using Integration.Shared.DTO.Security;
namespace Integration.Application.Mappings.Security
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Integration.Core.Entities.Security.Application, ApplicationDTO>()
                .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.ApplicationId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            IMappingExpression<ApplicationDTO, Integration.Core.Entities.Security.
                Application> mappingExpression = CreateMap<ApplicationDTO, Integration.Core.Entities.Security.Application>()
                .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.ApplicationId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.Modules, opt => opt.Ignore());
        }
    }
}