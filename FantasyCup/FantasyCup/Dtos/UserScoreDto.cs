using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyCup.Dtos
{
    public class UserScoreDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int CountCorrectSign { get; set; }
        public int CountCorrectScore { get; set; }
        public int CountCorrectProgress { get; set; }
        public int GroupGamePoints { get; set; }
        public int EliminationGamePoints { get; set; }
        public int CountGames { get; set; }
        public int PotentialChampionPoints { get; set; }
        public int PotentialScorerPoints { get; set; }
        public int ChampionPoints { get; set; }
        public int ScorerPoints { get; set; }
        public int Position { get; set; }
        public int? PreviousPosition { get; set; }
        public bool HasPotMoney { get; set; }
        public bool Paid { get; set; }
        public int Points
        {
            get
            {
                return this.ChampionPoints + this.ScorerPoints + this.GroupGamePoints + this.EliminationGamePoints;
            }
        }
    }
}
