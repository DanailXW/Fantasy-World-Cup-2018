using System;
using System.Collections.Generic;

namespace FantasyCup.Model
{
    public partial class Team
    {
        public Team()
        {
            GameTeamA = new HashSet<Game>();
            GameTeamB = new HashSet<Game>();
            GameUserBet = new HashSet<GameUserBet>();
            Player = new HashSet<Player>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Game> GameTeamA { get; set; }
        public ICollection<Game> GameTeamB { get; set; }
        public ICollection<GameUserBet> GameUserBet { get; set; }
        public ICollection<Player> Player { get; set; }
    }
}
