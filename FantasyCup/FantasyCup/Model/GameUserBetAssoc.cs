using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyCup.Model
{
    public class GameUserBetAssoc
    {
        public int GameId { get; set; }
        public int UserId { get; set; }
        public int? WinningTeamId { get; set; }
        public int? ScoreA { get; set; }
        public int? ScoreB { get; set; }
        public DateTime? PlaceDate { get; set; }
        public bool CorrectSign { get; set; }
        public bool CorrectScore { get; set; }
        public bool CorrectProgress { get; set; }

        public Game Game { get; set; }
        public User User { get; set; }
    }
}
