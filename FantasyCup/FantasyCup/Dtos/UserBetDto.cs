using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyCup.Dtos
{
    public class UserBetDto
    {
        public string GameId { get; set; }
        public int? WinningTeamId { get; set; }
        public int ScoreA { get; set; }
        public int ScoreB { get; set; }
    }
}
