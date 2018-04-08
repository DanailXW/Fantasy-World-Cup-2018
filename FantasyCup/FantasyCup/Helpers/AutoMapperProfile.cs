using AutoMapper;
using FantasyCup.Model;
using FantasyCup.Dtos;

namespace FantasyCup.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<League, LeagueDto>();
            CreateMap<LeagueDto, League>();

            CreateMap<LeagueUser, LeagueUserDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<LeagueUserDto, LeagueUser>();
        }
    }
}
