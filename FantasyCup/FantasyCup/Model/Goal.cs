using System;
using System.Collections.Generic;

namespace FantasyCup.Model
{
    public partial class Goal
    {
        public int Id { get; set; }
        public int ResultId { get; set; }
        public int? PlayerId { get; set; }
        public int? TypeId { get; set; }
        public string Minute { get; set; }

        public Player Player { get; set; }
        public Result Result { get; set; }
        public GoalType Type { get; set; }
    }
}
