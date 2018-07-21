using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyCup.Dtos
{
    public class GameOtherUserBetDto
    {
        public int GameId { get; set; }
        public int ScoreA { get; set; }
        public int ScoreB { get; set; }
        public int? WinningTeamId { get; set; }
        public DateTime PlaceDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
