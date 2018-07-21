using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyCup.Dtos
{
    public class GameUserBetDto
    {
        public GameDto Game { get; set; }
        public int GameId { get; set; }
        public int? WinningTeamId { get; set; }
        public int? ScoreA { get; set; }
        public int? ScoreB { get; set; }
        public DateTime? PlaceDate { get; set; }
        public bool CorrectSign { get; set; }
        public bool CorrectScore { get; set; }
        public bool CorrectProgress { get; set; }
        public bool CanViewOthersBets { get; set; }
        public string UserName { get; set; }
    }
}
