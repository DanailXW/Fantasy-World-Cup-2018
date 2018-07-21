using System;
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

            CreateMap<LeagueUserInfo, LeagueUserInfoDto>();

            //CreateMap<GameUserBet, UserBetDto>()
            //    .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<GameUserBetAssoc, GameUserBetDto>()
                .ForMember(dest => dest.CanViewOthersBets, opt => opt.MapFrom(src => (DateTime.UtcNow >= src.Game.StartDate)))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null));

            CreateMap<UserBetDto, GameUserBet>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Game, opt => opt.Ignore())
                .ForMember(dest => dest.WinningTeamId, opt => opt.MapFrom(src => src.WinningTeamId.HasValue ? src.WinningTeamId.Value : -1));

            CreateMap<Team, TeamDto>();
            CreateMap<Player, PlayerDto>();

            CreateMap<CompetitionUserBetDto, CompetitionUserBet>()
                .ForMember(dest => dest.BetType, opt => opt.MapFrom(src => new BetType { Name = src.BetType }));

            CreateMap<UserScore, UserScoreDto>();

            CreateMap<Result, ResultDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.Name));

            CreateMap<Game, GameDto>()
                .ForMember(dest => dest.Stage, opt => opt.MapFrom(src => src.Stage.Name))
                .ForMember(dest => dest.StageType, opt => opt.MapFrom(src => src.Stage.StageType.Name))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.Name));

            CreateMap<PlayerStats, PlayerStatsDto>();

            CreateMap<GameUserBet, GameOtherUserBetDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<Stage, StageDto>()
                .ForMember(dest => dest.StageType, opt => opt.MapFrom(src => src.StageType.Name));
        }
    }
}
