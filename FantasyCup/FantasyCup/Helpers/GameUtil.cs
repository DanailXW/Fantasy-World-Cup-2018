using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FantasyCup.Model;

namespace FantasyCup.Helpers
{
    public static class GameUtil
    {
        public static bool IsSignCorrect(GameUserBet bet)
        {
            if (bet.Game.State.Name != "Finished")
                return false;

            var result = bet.Game.Result.SingleOrDefault(x => x.Type.Name == "REGULAR");
            if ((result.ScoreA > result.ScoreB) == (bet.ScoreA > bet.ScoreB) && (result.ScoreA < result.ScoreB) == (bet.ScoreA < bet.ScoreB))
                return true;

            return false;
        }

        public static bool IsScoreCorrect(GameUserBet bet)
        {
            if (bet.Game.State.Name != "Finished")
                return false;

            var result = bet.Game.Result.SingleOrDefault(x => x.Type.Name == "REGULAR");
            if (result.ScoreA == bet.ScoreA && result.ScoreB == bet.ScoreB)
                return true;

            return false;
        }

        public static bool IsProgressCorrect(GameUserBet bet)
        {
            if (bet.Game.State.Name != "Finished")
                return false;
            if (bet.Game.Stage.StageType.Name != "Elimination")
                return false;
            if (!bet.WinningTeamId.HasValue)
                return false;

            var result = bet.Game.Result.SingleOrDefault(x => x.Type.Name == "REGULAR");
            if (result.ScoreA != result.ScoreB)
                return false;

            var totalScoreA = bet.Game.Result.Sum(x => x.ScoreA);
            var totalScoreB = bet.Game.Result.Sum(x => x.ScoreB);

            var teamId = (totalScoreA > totalScoreB) ? bet.Game.TeamAid : bet.Game.TeamBid;

            return teamId == bet.WinningTeamId;
        }

        public static bool IsChampionCorrect(CompetitionUserBet bet, Game finalGame)
        {
            if (finalGame == null)
                return false;

            var totalScoreA = finalGame.Result.Sum(x => x.ScoreA);
            var totalScoreB = finalGame.Result.Sum(x => x.ScoreB);

            return (totalScoreA > totalScoreB ? finalGame.TeamAid : finalGame.TeamBid) == bet.SelectionId;
        }

        public static bool IsTopScorerCorrect(CompetitionUserBet bet, Game finalGame, IEnumerable<PlayerStats> playerStats)
        {
            if (finalGame == null)
                return false;

            var maxGoals = playerStats.Max(x => x.Goals);
            return playerStats.Any(x => x.Goals == maxGoals && x.Player.Id == bet.SelectionId);
        }
    }
}
