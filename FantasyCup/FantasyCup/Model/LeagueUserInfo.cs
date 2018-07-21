using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyCup.Model
{
    public class LeagueUserInfo
    {
        public League league { get; set; }
        public bool isAdmin { get; set; }
        public bool canLeave { get; set; }
    }
}
