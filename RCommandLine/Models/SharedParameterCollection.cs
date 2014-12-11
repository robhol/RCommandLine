using System.Collections.Generic;

namespace RCommandLine
{
    class SharedParameterCollection
    {
        public List<Flag> Flags { get; private set; }

        public List<Argument> Arguments { get; private set; } 
    }
}
