using System;
using System.Collections.Generic;

namespace FantasyCup.Model
{
    public partial class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TeamId { get; set; }

        public Team Team { get; set; }
    }
}
