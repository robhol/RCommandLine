using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{
    public class MissingValuesException : Exception
    {

        public IEnumerable<string> Parameter { get; private set; }

        internal MissingValuesException(IEnumerable<ParameterElement> p)
        {
            Parameter = p.Select(pa => pa.Name);
        }

    }
}
