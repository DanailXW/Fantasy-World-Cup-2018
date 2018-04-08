using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyCup.Helpers
{
    public class FantasyException : Exception
    {
        public FantasyException() : base() { }
        public FantasyException(string message) : base(message) { }
    }
}
