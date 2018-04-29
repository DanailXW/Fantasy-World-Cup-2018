using System.Linq;
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

            CreateMap<Game, GameUserBetDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ScoreA, opt => opt.MapFrom(src => src.GameUserBet.FirstOrDefault().ScoreA))
                .ForMember(dest => dest.ScoreB, opt => opt.MapFrom(src => src.GameUserBet.FirstOrDefault().ScoreB))
                .ForMember(dest => dest.Stage, opt => opt.MapFrom(src => src.Stage.Name))
                .ForMember(dest => dest.StageType, opt => opt.MapFrom(src => src.Stage.StageType.Name))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.TeamA, opt => opt.MapFrom(src => src.TeamA.Name))
                .ForMember(dest => dest.TeamB, opt => opt.MapFrom(src => src.TeamB.Name))
                .ForMember(dest => dest.TeamAId, opt => opt.MapFrom(src => src.TeamAid))
                .ForMember(dest => dest.TeamBId, opt => opt.MapFrom(src => src.TeamBid))
                .ForMember(dest => dest.WinningTeamId, opt => opt.MapFrom(src => src.GameUserBet.FirstOrDefault().WinningTeamId));

            CreateMap<GameUserBetDto, GameUserBet>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ScoreA, opt => opt.MapFrom(src => src.ScoreA.HasValue ? src.ScoreA.Value : -1))
                .ForMember(dest => dest.ScoreB, opt => opt.MapFrom(src => src.ScoreB.HasValue ? src.ScoreB.Value : -1))
                .ForMember(dest => dest.WinningTeamId, opt => opt.MapFrom(src => src.WinningTeamId.HasValue ? src.WinningTeamId.Value : -1));

            CreateMap<Team, TeamDto>();
            CreateMap<Player, PlayerDto>();
        }
    }
}
