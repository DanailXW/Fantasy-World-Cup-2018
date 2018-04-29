using System;
using System.Collections.Generic;

namespace FantasyCup.Model
{
    public partial class Competition
    {
        public Competition()
        {
            CompetitionUserBet = new HashSet<CompetitionUserBet>();
            Stage = new HashSet<Stage>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<CompetitionUserBet> CompetitionUserBet { get; set; }
        public ICollection<Stage> Stage { get; set; }
    }
}
