namespace RCommandLine.Models
{
    using System.Collections.Generic;

    class SharedParameterCollection
    {
        public List<Flag> Flags { get; private set; }

        public List<Argument> Arguments { get; private set; } 
    }
}
