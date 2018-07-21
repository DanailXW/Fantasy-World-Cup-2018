using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyCup.Model
{
    public class GroupStandings
    {
        public int TeamId { get; set; }
        public int StageId { get; set; }
        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }

        public Team Team { get; set; }
        public Stage Stage { get; set; }
    }
}
