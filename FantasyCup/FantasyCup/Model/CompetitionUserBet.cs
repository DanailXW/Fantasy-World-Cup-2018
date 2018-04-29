using System;
using System.Collections.Generic;

namespace FantasyCup.Model
{
    public partial class CompetitionUserBet
    {
        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public int UserId { get; set; }
        public int SelectionId { get; set; }
        public DateTime PlaceDate { get; set; }
        public int BetTypeId { get; set; }

        public BetType BetType { get; set; }
        public Competition Competition { get; set; }
        public User User { get; set; }
    }
}
