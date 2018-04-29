using System;
using System.Collections.Generic;

namespace FantasyCup.Model
{
    public partial class GameUserBet
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int UserId { get; set; }
        public int? WinningTeamId { get; set; }
        public int ScoreA { get; set; }
        public int ScoreB { get; set; }
        public DateTime PlaceDate { get; set; }

        public Game Game { get; set; }
        public User User { get; set; }
        public Team WinningTeam { get; set; }
    }
}
