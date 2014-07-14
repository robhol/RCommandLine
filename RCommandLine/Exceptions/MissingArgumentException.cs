using System;
using System.Collections.Generic;
using System.Linq;

namespace RCommandLine
{
    public class MissingArgumentException : Exception
    {

        public IEnumerable<string> Parameters { get; private set; }

        internal MissingArgumentException(string message, IEnumerable<CommonParameterElement> p) : base(message)
        {
            Parameters = p.Select(pa => pa.Name);
        }

    }
}
