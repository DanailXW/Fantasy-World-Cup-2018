using System;
using System.Collections.Generic;

namespace FantasyCup.Model
{
    public partial class Stage
    {
        public Stage()
        {
            Game = new HashSet<Game>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int StageTypeId { get; set; }
        public int CompetitionId { get; set; }

        public Competition Competition { get; set; }
        public StageType StageType { get; set; }
        public ICollection<Game> Game { get; set; }
    }
}
