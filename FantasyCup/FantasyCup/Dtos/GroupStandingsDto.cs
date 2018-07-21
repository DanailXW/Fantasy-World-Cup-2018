using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyCup.Dtos
{
    public class GroupStandingsDto
    {
        public TeamDto Team { get; set; }
        public StageDto Stage { get; set; }
        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int Points
        {
            get
            {
                return 3 * this.Wins + 1 * this.Draws;
            }
        }
    }
}
