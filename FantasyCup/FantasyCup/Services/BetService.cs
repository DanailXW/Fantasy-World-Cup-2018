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
        void PlaceBetCompetition(int userId, int competitionId, CompetitionUserBet[] userBets);
        IEnumerable<GameUserBetAssoc> GetGamesToBet(int userId);
        IEnumerable<CompetitionUserBet> GetCompetitionBets(int userId, int competitionId);
        IEnumerable<GameUserBetAssoc> GetOthersBets(int userId, int gameId);
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

                if (game.StartDate < DateTime.UtcNow)
                    throw new FantasyException("Betting after the game start is not allowed");
                if (game.Stage.StageType.Name == "Elimination" && bet.ScoreA == bet.ScoreB && bet.WinningTeamId == -1)
                    throw new FantasyException("You need to specify which team will progress in case of draw");
                if (bet.ScoreA == -1 && bet.ScoreB == -1) //prediction is deleted
                {
                    var userBet = _context.GameUserBet.FirstOrDefault(x => x.UserId == userId && x.GameId == game.Id);
                    if (userBet != null)
                        _context.GameUserBet.Remove(userBet);

                }
                else
                {
                    if (bet.ScoreA < 0 || bet.ScoreB < 0)
                        throw new FantasyException("Invalid score");

                    var userBet = _context.GameUserBet.FirstOrDefault(x => x.UserId == userId && x.GameId == game.Id);

                    if (userBet == null)
                    {
                        userBet = new GameUserBet();
                        userBet.UserId = userId;
                        userBet.GameId = game.Id;
                        userBet.ScoreA = bet.ScoreA;
                        userBet.ScoreB = bet.ScoreB;
                        userBet.PlaceDate = DateTime.UtcNow;

                        if (game.Stage.StageType.Name == "Elimination")
                        {
                            if (bet.ScoreA == bet.ScoreB)
                                userBet.WinningTeamId = bet.WinningTeamId;
                            else
                                userBet.WinningTeamId = null;
                        }

                        _context.GameUserBet.Add(userBet);
                    }
                    else
                    {
                        if (userBet.ScoreA != bet.ScoreA || userBet.ScoreB != bet.ScoreB)
                        {
                            userBet.ScoreA = bet.ScoreA;
                            userBet.ScoreB = bet.ScoreB;
                            userBet.PlaceDate = DateTime.UtcNow;
                        }

                        if (game.Stage.StageType.Name == "Elimination")
                        {
                            var teamId = userBet.WinningTeamId;
                            if (bet.ScoreA == bet.ScoreB)
                                teamId = bet.WinningTeamId;
                            else
                                teamId = null;

                            if (teamId != userBet.WinningTeamId)
                            {
                                userBet.WinningTeamId = teamId;
                                userBet.PlaceDate = DateTime.UtcNow;
                            }
                        }

                        if (_context.Entry(userBet).State != EntityState.Unchanged)
                            _context.GameUserBet.Update(userBet);
                    }
                }
                
            }


            _context.SaveChanges();

        }

        public void PlaceBetCompetition(int userId, int competitionId, CompetitionUserBet[] userBets)
        {
            if(!_context.User.Any(x => x.Id == userId))
                throw new FantasyException("User not found");
            if (!_context.Competition.Any(x => x.Id == competitionId))
                throw new FantasyException("Competition not found");

            var finalDate = _context.Competition.Single(x => x.Id == competitionId).EndDate;
            if (_context.Game.Any(x => x.Stage.CompetitionId == competitionId && x.StartDate > finalDate))
                finalDate = _context.Game.Where(x => x.Stage.CompetitionId == competitionId).Max(x => x.StartDate);

            if (finalDate < DateTime.UtcNow)
                throw new FantasyException("Placing a competition bet after the final game start is not allowed");

            foreach (var bet in userBets)
            {
                if (!_context.BetType.Any(x => x.Name == bet.BetType.Name))
                    throw new FantasyException("Unsupported bet type");                

                if (bet.BetType.Name == "COMPETITION_CHAMPION")
                {
                    if (!_context.Team.Any(x => x.Id == bet.SelectionId))
                        throw new FantasyException("Team not found");
                }
                else
                {
                    if (!_context.Player.Any(x => x.Id == bet.SelectionId))
                        throw new FantasyException("Player not found");
                }

                var betTypeId = _context.BetType.Single(x => x.Name == bet.BetType.Name).Id;
                var userBet = _context.CompetitionUserBet.FirstOrDefault(x => x.UserId == userId && x.CompetitionId == competitionId && x.BetTypeId == betTypeId);

                if (userBet == null)
                {
                    userBet = new CompetitionUserBet();
                    userBet.UserId = userId;
                    userBet.CompetitionId = competitionId;
                    userBet.BetTypeId = betTypeId;
                    userBet.PlaceDate = DateTime.UtcNow;
                    userBet.SelectionId = bet.SelectionId;

                    _context.CompetitionUserBet.Add(userBet);
                }
                else if (userBet.SelectionId != bet.SelectionId)
                {
                    userBet.PlaceDate = DateTime.UtcNow;
                    userBet.SelectionId = bet.SelectionId;

                    _context.CompetitionUserBet.Update(userBet);
                }
            }
            
            _context.SaveChanges();
        }


        public IEnumerable<GameUserBetAssoc> GetGamesToBet(int userId)
        {
            return _context.GameUserBetAssoc.FromSql("SELECT * FROM dbo.utfn_getGameUserBet(" + userId.ToString() + ")")
                                            .Include(x => x.Game)
                                                .ThenInclude(x => x.TeamA)
                                            .Include(x => x.Game)
                                                .ThenInclude(x => x.TeamB)
                                            .Include(x => x.Game)
                                                .ThenInclude(x => x.Stage)
                                                    .ThenInclude(x => x.StageType)
                                            .Include(x => x.Game)
                                                .ThenInclude(x => x.State);
            //return _context.Game
            //                .Include(x => x.TeamA)
            //                .Include(x => x.TeamB)
            //                .Include(x => x.GameUserBet)
            //                .Include(x => x.Stage)
            //                    .ThenInclude(x => x.StageType)
            //                .AsNoTracking()
            //                .SelectMany(
            //                        collectionSelector: game => game.GameUserBet.Where(x => x.UserId == userId).DefaultIfEmpty(),
            //                        resultSelector: (game, userbet) => new Game
            //                        {
            //                            Id = game.Id,
            //                            Result = game.Result,
            //                            Stage = game.Stage,
            //                            TeamA = game.TeamA,
            //                            TeamAid = game.TeamAid,
            //                            TeamB = game.TeamB,
            //                            TeamBid = game.TeamBid,
            //                            StartDate = game.StartDate,
            //                            GameUserBet = new List<GameUserBet>() { userbet }
            //                        });

        }

        public IEnumerable<CompetitionUserBet> GetCompetitionBets(int userId, int competitionId)
        {
            return _context.CompetitionUserBet.Include(x => x.BetType).Where(x => x.UserId == userId);
        }

        public IEnumerable<GameUserBetAssoc> GetOthersBets(int userId, int gameId)
        {
            if (!_context.User.Any(x => x.Id == userId))
                throw new FantasyException("User not found");

            var game = _context.Game.Find(gameId);
            if (game.StartDate > DateTime.UtcNow)
                throw new FantasyException("You can't view other players' bets before the game has started.");

            return _context.GameUserBetAssoc.FromSql("SELECT * FROM dbo.vw_GameUserBet WHERE GameId = " + gameId.ToString() + " AND UserId <> " + userId.ToString())
                                            .Include(x => x.User)
                                            .Include(x => x.Game)
                                                .ThenInclude(x => x.State);
        }
    }
}
