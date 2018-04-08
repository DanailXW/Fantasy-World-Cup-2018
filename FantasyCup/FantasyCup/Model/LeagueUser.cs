using System;
using System.Collections.Generic;

namespace FantasyCup.Model
{
    public partial class LeagueUser
    {
        public int LeagueId { get; set; }
        public int UserId { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime JoinDate { get; set; }
        public bool Paid { get; set; }

        public League League { get; set; }
        public User User { get; set; }
    }
}
