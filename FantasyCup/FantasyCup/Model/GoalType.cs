using System;
using System.Collections.Generic;

namespace FantasyCup.Model
{
    public partial class GoalType
    {
        public GoalType()
        {
            Goal = new HashSet<Goal>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsElligibleForRank { get; set; }

        public ICollection<Goal> Goal { get; set; }
    }
}
