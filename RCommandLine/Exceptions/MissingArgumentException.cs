using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{
    public class MissingArgumentException : Exception
    {

        public IEnumerable<string> Parameters { get; private set; }

        internal MissingArgumentException(string message, IEnumerable<ArgumentElement> p) : base(message)
        {
            Parameters = p.Select(pa => pa.Name);
        }

    }
}
