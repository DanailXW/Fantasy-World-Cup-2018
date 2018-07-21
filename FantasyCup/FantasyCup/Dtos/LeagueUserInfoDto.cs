using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyCup.Dtos
{
    public class LeagueUserInfoDto
    {
        public LeagueDtoSecure league { get; set; }
        public bool isAdmin { get; set; }
        public bool canLeave { get; set; }
    }
}
