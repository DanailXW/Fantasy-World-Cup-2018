using System;
using System.Collections.Generic;

namespace FantasyCup.Model
{
    public partial class GameState
    {
        public GameState()
        {
            Game = new HashSet<Game>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Game> Game { get; set; }
    }
}
