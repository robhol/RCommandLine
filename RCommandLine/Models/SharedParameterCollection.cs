using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{
    class SharedParameterCollection
    {
        public List<Flag> Flags { get; private set; }

        public List<Argument> Arguments { get; private set; } 
    }
}
