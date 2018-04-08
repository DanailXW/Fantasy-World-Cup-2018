using System;
using System.Collections.Generic;

namespace FantasyCup.Model
{
    public partial class League
    {
        public League()
        {
            LeagueUser = new HashSet<LeagueUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public bool HasPotMoney { get; set; }
        public decimal PotAmount { get; set; }
        public DateTime Created { get; set; }

        public ICollection<LeagueUser> LeagueUser { get; set; }
    }
}
