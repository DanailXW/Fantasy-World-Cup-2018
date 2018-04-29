using System;
using System.Collections.Generic;

namespace FantasyCup.Model
{
    public partial class ResultType
    {
        public ResultType()
        {
            Result = new HashSet<Result>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Result> Result { get; set; }
    }
}
