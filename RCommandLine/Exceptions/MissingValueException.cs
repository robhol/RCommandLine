using System;
using System.Collections.Generic;
using System.Linq;

namespace RCommandLine
{
    public class MissingValueException : Exception
    {

        public IEnumerable<string> Parameters { get; private set; }

        internal MissingValueException(string message, IEnumerable<ArgumentElement> p) : base(message)
        {
            Parameters = p.Select(pa => pa.Name);
        }

    }
}
