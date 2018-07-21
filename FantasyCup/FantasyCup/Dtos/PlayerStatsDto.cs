using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyCup.Dtos
{
    public class PlayerStatsDto
    {
        public PlayerDto Player { get; set; }
        public int Goals { get; set; }
    }
}
