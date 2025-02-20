using AutoMapper;

using Integration.Core.Entities.Audit;
using Integration.Shared.DTO.Audit;

namespace Integration.Application.Mappings.Audit
{
    public class LogProfile : Profile
    {
        public LogProfile()
        {
            CreateMap<LogDTO, Log>()
                .ForMember(dest => dest.LogId, opt => opt.Condition(src => src.LogId != Guid.Empty));

            CreateMap<Log, LogDTO>().ReverseMap();
        }
    }
}