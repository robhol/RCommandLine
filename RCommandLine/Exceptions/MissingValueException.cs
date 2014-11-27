using System;
using System.Collections.Generic;

namespace RCommandLine
{
    public class MissingValueException : Exception
    {

        public IEnumerable<string> Parameters { get; private set; }

        internal MissingValueException(string message, IEnumerable<string> parameters) : base(message)
        {
            Parameters = parameters;
        }

    }
}
