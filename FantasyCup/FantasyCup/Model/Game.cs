using System;
using System.Collections.Generic;

namespace FantasyCup.Model
{
    public partial class Game
    {
        public Game()
        {
            GameUserBet = new HashSet<GameUserBet>();
            Result = new HashSet<Result>();
        }

        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public int TeamAid { get; set; }
        public int TeamBid { get; set; }
        public int StageId { get; set; }

        public Stage Stage { get; set; }
        public Team TeamA { get; set; }
        public Team TeamB { get; set; }
        public ICollection<GameUserBet> GameUserBet { get; set; }
        public ICollection<Result> Result { get; set; }
    }
}
