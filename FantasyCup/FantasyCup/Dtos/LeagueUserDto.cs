using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyCup.Dtos
{
    public class LeagueUserDto
    {
        public int LeagueId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public bool Paid { get; set; }
    }
}
