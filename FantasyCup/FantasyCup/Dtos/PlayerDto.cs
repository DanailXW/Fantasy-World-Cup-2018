using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyCup.Dtos
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TeamDto Team { get; set; }
    }
}
