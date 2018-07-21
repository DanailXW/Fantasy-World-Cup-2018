using System;
using System.Collections.Generic;

namespace FantasyCup.Model
{
    public partial class StageType
    {
        public StageType()
        {
            Stage = new HashSet<Stage>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int SignPoints { get; set; }
        public int ScorePoints { get; set; }
        public int ProgressPoints { get; set; }

        public ICollection<Stage> Stage { get; set; }
    }
}
