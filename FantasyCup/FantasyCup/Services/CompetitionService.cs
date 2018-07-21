using System;
using System.Collections.Generic;
using System.Linq;
using FantasyCup.Model;
using Microsoft.EntityFrameworkCore;

namespace FantasyCup.Services
{
    public interface ICompetitionService
    {
        IEnumerable<Team> GetTeams(int competitionId);
        IEnumerable<Player> GetPlayers(int competitionId);
        IEnumerable<Player> GetPlayersByTeam(int competitionId, int teamId);
        IEnumerable<Game> GetGames(int competitionId);
        IEnumerable<PlayerStats> GetPlayerStats(int competitionId);
        IEnumerable<GroupStandings> GetGroupStandings(int competitionId);
    }

    public class CompetitionService : ICompetitionService
    {
        private FantasyCupContext _context;

        public CompetitionService(FantasyCupContext context)
        {
            _context = context;
        }

        public IEnumerable<Team> GetTeams(int competitionId)
        {
            return _context.Team.Where(x => x.GameTeamA.Select(g => g.Stage.CompetitionId).Contains(competitionId) 
                                            || x.GameTeamB.Select(g => g.Stage.CompetitionId).Contains(competitionId));
        }

        public IEnumerable<Player> GetPlayers(int competitionId)
        {
            return _context.Player.Include(x => x.Team);
        }

        public IEnumerable<Player> GetPlayersByTeam(int competitionId, int teamId)
        {
            return _context.Player.Include(x => x.Team).Where(x => x.TeamId == teamId);
        }

        public IEnumerable<Game> GetGames(int competitionId)
        {
            return _context.Game.Include(x => x.Stage)
                                    .ThenInclude(x => x.StageType)
                                .Include(x => x.State)
                                .Include(x => x.Result)
                                    .ThenInclude(x => x.Type)
                                .Include(x => x.Result)
                                    .ThenInclude(x => x.Goal)
                                        .ThenInclude(x => x.Player)
                                .Include(x => x.Result)
                                    .ThenInclude(x => x.Goal)
                                        .ThenInclude(x => x.Type)
                                .Include(x => x.TeamA)
                                .Include(x => x.TeamB)
                                .AsNoTracking();
        }

        public IEnumerable<PlayerStats> GetPlayerStats(int competitionId)
        {
            return _context.Goal
                            .Include(x => x.Type)
                            .Include(x => x.Player)
                                .ThenInclude(x => x.Team)
                            .Where(x => x.Type.IsElligibleForRank && x.PlayerId.HasValue)
                            .GroupBy(x => x.Player)
                            .Select(x => new PlayerStats { Player = x.Key, Goals = x.Count() });
        }

        public IEnumerable<GroupStandings> GetGroupStandings(int competitionId)
        {
            return _context.GroupStandings
                            .FromSql("SELECT * FROM dbo.vw_GroupStandings")
                            .Include(x => x.Team)
                            .Include(x => x.Stage)
                                .ThenInclude(x => x.StageType);
        }
    }
}
