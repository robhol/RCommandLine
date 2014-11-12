using System;
using System.Collections.Generic;
using System.Linq;

namespace RCommandLine
{
    public class MissingArgumentException : Exception
    {

        public IEnumerable<string> Parameters { get; private set; }

        internal MissingArgumentException(string message, IEnumerable<string> parameters) : base(message)
        {
            Parameters = parameters;
        }

    }
}
