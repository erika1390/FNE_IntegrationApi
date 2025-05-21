using AutoMapper;

using Integration.Core.Entities.Parametric;
using Integration.Shared.DTO.Parametric;

namespace Integration.Application.Mappings.Parametric
{
    public class IdentificationDocumentTypeProfile : Profile
    {
        public IdentificationDocumentTypeProfile()
        {
            CreateMap<IdentificationDocumentType, IdentificationDocumentTypeDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Abbreviation, opt => opt.MapFrom(src => src.Abbreviation))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ReverseMap();
        }
    }
}