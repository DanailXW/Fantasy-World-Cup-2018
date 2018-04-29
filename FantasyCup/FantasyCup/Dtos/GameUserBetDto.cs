using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyCup.Dtos
{
    public class GameUserBetDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public string Stage { get; set; }
        public string StageType { get; set; }
        public string TeamA { get; set; }
        public string TeamB { get; set; }
        public int TeamAId { get; set; }
        public int TeamBId { get; set; }
        public int? ScoreA { get; set; }
        public int? ScoreB { get; set; }
        public int? WinningTeamId { get; set; }

    }
}
