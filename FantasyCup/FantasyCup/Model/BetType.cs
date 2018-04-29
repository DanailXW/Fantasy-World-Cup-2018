using System;
using System.Collections.Generic;

namespace FantasyCup.Model
{
    public partial class BetType
    {
        public BetType()
        {
            CompetitionUserBet = new HashSet<CompetitionUserBet>();
            UserBet = new HashSet<UserBet>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<CompetitionUserBet> CompetitionUserBet { get; set; }
        public ICollection<UserBet> UserBet { get; set; }
    }
}
