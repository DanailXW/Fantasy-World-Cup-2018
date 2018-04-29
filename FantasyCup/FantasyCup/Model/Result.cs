using System;
using System.Collections.Generic;

namespace FantasyCup.Model
{
    public partial class Result
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int ScoreA { get; set; }
        public int ScoreB { get; set; }
        public int TypeId { get; set; }
        public bool IsActual { get; set; }

        public Game Game { get; set; }
        public ResultType Type { get; set; }
    }
}
