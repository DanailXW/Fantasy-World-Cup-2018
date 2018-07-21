using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyCup.Dtos
{
    public class GameDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public string Stage { get; set; }
        public string StageType { get; set; }
        public string State { get; set; }
        public TeamDto TeamA { get; set; }
        public TeamDto TeamB { get; set; }
        public ICollection<ResultDto> Result { get; set; }
    }
}
