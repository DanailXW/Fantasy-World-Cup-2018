using System;
using System.Collections.Generic;

namespace FantasyCup.Model
{
    public partial class Player
    {
        public Player()
        {
            Goal = new HashSet<Goal>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int TeamId { get; set; }

        public Team Team { get; set; }
        public ICollection<Goal> Goal { get; set; }
    }
}
