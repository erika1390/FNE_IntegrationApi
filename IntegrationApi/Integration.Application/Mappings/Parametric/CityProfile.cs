using AutoMapper;

using Integration.Core.Entities.Parametric;
using Integration.Shared.DTO.Parametric;

namespace Integration.Application.Mappings.Parametric
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityDTO>()
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.CodeDane, opt => opt.MapFrom(src => src.CodeDane))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ReverseMap();
        }
    }
}