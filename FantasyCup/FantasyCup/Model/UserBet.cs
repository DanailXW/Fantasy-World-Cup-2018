using System;
using System.Collections.Generic;

namespace FantasyCup.Model
{
    public partial class UserBet
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime PlaceDate { get; set; }
        public int BetTypeId { get; set; }
        public int EventId { get; set; }
        public int SelectionId { get; set; }

        public BetType BetType { get; set; }
        public User User { get; set; }
    }
}
