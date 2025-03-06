using AutoMapper;
using Integration.Core.Entities.Security;
using Integration.Shared.DTO.Security;
namespace Integration.Core.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.NormalizedUserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.NormalizedEmail))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash))
                .ForMember(dest => dest.SecurityStamp, opt => opt.MapFrom(src => src.SecurityStamp))
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.MapFrom(src => src.PhoneNumberConfirmed))
                .ForMember(dest => dest.TwoFactorEnabled, opt => opt.MapFrom(src => src.TwoFactorEnabled))
                .ForMember(dest => dest.LockoutEnd, opt => opt.MapFrom(src => src.LockoutEnd))
                .ForMember(dest => dest.LockoutEnabled, opt => opt.MapFrom(src => src.LockoutEnabled))
                .ForMember(dest => dest.AccessFailedCount, opt => opt.MapFrom(src => src.AccessFailedCount))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.NormalizedUserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.NormalizedEmail))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash))
                .ForMember(dest => dest.SecurityStamp, opt => opt.MapFrom(src => src.SecurityStamp))
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.MapFrom(src => src.PhoneNumberConfirmed))
                .ForMember(dest => dest.TwoFactorEnabled, opt => opt.MapFrom(src => src.TwoFactorEnabled))
                .ForMember(dest => dest.LockoutEnd, opt => opt.MapFrom(src => src.LockoutEnd))
                .ForMember(dest => dest.LockoutEnabled, opt => opt.MapFrom(src => src.LockoutEnabled))
                .ForMember(dest => dest.AccessFailedCount, opt => opt.MapFrom(src => src.AccessFailedCount))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));
        }
    }
}