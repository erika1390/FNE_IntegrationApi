using AutoMapper;

using Integration.Core.Entities.Audit;
using Integration.Shared.DTO.Audit;

namespace Integration.Application.Mappings.Audit
{
    public class LogProfile : Profile
    {
        public LogProfile()
        {
            CreateMap<LogDTO, Log>().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Log, LogDTO>();
        }
    }
}