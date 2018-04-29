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
        IEnumerable<Player> GetPlayersByTeam(int competitionId, int teamId);
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

        public IEnumerable<Player> GetPlayersByTeam(int competitionId, int teamId)
        {
            return _context.Player.Where(x => x.TeamId == teamId);
        }
    }
}
