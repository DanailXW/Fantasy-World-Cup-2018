using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyCup.Model;
using FantasyCup.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FantasyCup.Services
{
    public interface IBetService
    {
        void PlaceBetGameResult(int userId, GameUserBet[] bets);
        void PlaceBetCompetition(int userId, int competitionId, string betType, int selectionId);
        IEnumerable<Game> GetGamesToBet(int userId);
    }

    public class BetService : IBetService
    {
        private FantasyCupContext _context;

        public BetService(FantasyCupContext context)
        {
            _context = context;
        }

        public void PlaceBetGameResult(int userId, GameUserBet[] bets)
        {
            if (!_context.User.Any(x => x.Id == userId))
                throw new FantasyException("User not found");

            foreach (var bet in bets)
            {
                if (!_context.Game.Any(x => x.Id == bet.GameId))
                    throw new FantasyException("Game not found");

                var game = _context.Game
                                    .Include(x => x.Stage)
                                        .ThenInclude(x => x.StageType)
                                    .FirstOrDefault(x => x.Id == bet.GameId);

                if (game.StartDate < DateTime.Now)
                    throw new FantasyException("Betting after the game start is not allowed");
                if (bet.ScoreA == -1 || bet.ScoreB == -1)
                    throw new FantasyException("Missing score value");
                if (game.Stage.StageType.Name == "Elimination" && bet.ScoreA == bet.ScoreB && bet.WinningTeamId == -1)
                    throw new FantasyException("You need to specify which team will progress");

                var userBet = _context.GameUserBet.FirstOrDefault(x => x.UserId == userId && x.GameId == game.Id);

                if (userBet == null)
                {
                    userBet = new GameUserBet();
                    userBet.UserId = userId;
                    userBet.GameId = game.Id;
                    userBet.ScoreA = bet.ScoreA;
                    userBet.ScoreB = bet.ScoreB;
                    userBet.PlaceDate = DateTime.Now;

                    if (game.Stage.StageType.Name == "Elimination")
                    {
                        if (bet.ScoreA == bet.ScoreB)
                            userBet.WinningTeamId = bet.WinningTeamId;
                        else
                            userBet.WinningTeamId = (bet.ScoreA > bet.ScoreB) ? game.TeamAid : game.TeamBid;
                    }

                    _context.GameUserBet.Add(userBet);
                }
                else
                {
                    if (userBet.ScoreA != bet.ScoreA || userBet.ScoreB != bet.ScoreB)
                    {
                        userBet.ScoreA = bet.ScoreA;
                        userBet.ScoreB = bet.ScoreB;
                        userBet.PlaceDate = DateTime.Now;
                    }

                    if (game.Stage.StageType.Name == "Elimination")
                    {
                        var teamId = userBet.WinningTeamId;
                        if (bet.ScoreA == bet.ScoreB)
                            teamId = bet.WinningTeamId;
                        else
                            teamId = (bet.ScoreA > bet.ScoreB) ? game.TeamAid : game.TeamBid;

                        if (teamId != userBet.WinningTeamId)
                        {
                            userBet.WinningTeamId = teamId;
                            userBet.PlaceDate = DateTime.Now;
                        }
                    }

                    if (_context.Entry(userBet).State != EntityState.Unchanged)
                        _context.GameUserBet.Update(userBet);
                }
            }


            _context.SaveChanges();

        }

        public void PlaceBetCompetition(int userId, int competitionId, string betType, int selectionId)
        {
            if(!_context.User.Any(x => x.Id == userId))
                throw new FantasyException("User not found");
            if (!_context.Competition.Any(x => x.Id == competitionId))
                throw new FantasyException("Competition not found");
            if (!_context.BetType.Any(x => x.Name == betType))
                throw new FantasyException("Unsupported bet type");

            var finalDate = _context.Competition.Single(x => x.Id == competitionId).EndDate;
            if (_context.Game.Any(x => x.Stage.CompetitionId == competitionId && x.StartDate > finalDate))
                finalDate = _context.Game.Where(x => x.Stage.CompetitionId == competitionId).Max(x => x.StartDate);

            if (finalDate < DateTime.Now)
                throw new FantasyException("Placing a competition bet after the final game start is not allowed");

            if (betType == "COMPETITION_CHAMPION")
            {
                if (!_context.Team.Any(x => x.Id == selectionId))
                    throw new FantasyException("Team not found");
            }
            else
            {
                if (!_context.Player.Any(x => x.Id == selectionId))
                    throw new FantasyException("Player not found");
            }

            var betTypeId = _context.BetType.Single(x => x.Name == betType).Id;
            var userBet = _context.CompetitionUserBet.FirstOrDefault(x => x.UserId == userId && x.CompetitionId == competitionId && x.BetTypeId == betTypeId);

            if (userBet == null)
            {
                userBet = new CompetitionUserBet();
                userBet.UserId = userId;
                userBet.CompetitionId = competitionId;
                userBet.BetTypeId = betTypeId;
                userBet.PlaceDate = DateTime.Now;
                userBet.SelectionId = selectionId;

                _context.CompetitionUserBet.Add(userBet);
            }
            else if (userBet.SelectionId != selectionId)
            {
                userBet.PlaceDate = DateTime.Now;
                userBet.SelectionId = selectionId;

                _context.CompetitionUserBet.Update(userBet);
            }

            _context.SaveChanges();
        }


        public IEnumerable<Game> GetGamesToBet(int userId)
        {
            return _context.Game
                            .Include(x => x.TeamA)
                            .Include(x => x.TeamB)
                            .Include(x => x.GameUserBet)
                            .Include(x => x.Stage)
                                .ThenInclude(x => x.StageType)
                            .AsNoTracking()
                            .SelectMany(
                                    collectionSelector: game => game.GameUserBet.Where(x => x.UserId == userId).DefaultIfEmpty(),
                                    resultSelector: (game, userbet) => new Game {
                                        Id = game.Id,
                                        Result = game.Result,
                                        Stage = game.Stage,
                                        TeamA = game.TeamA,
                                        TeamAid = game.TeamAid,
                                        TeamB = game.TeamB,
                                        TeamBid = game.TeamBid,
                                        StartDate = game.StartDate,
                                        GameUserBet = new List<GameUserBet>() { userbet }
                                    });
        }
    }
}
