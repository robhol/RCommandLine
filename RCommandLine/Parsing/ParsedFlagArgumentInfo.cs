using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{
    /// <summary>
    /// Temporary information object used in parsing
    /// </summary>
    class ParsedFlagArgumentInfo
    {
        public FlagElement Element { get; set; }

        public FlagMatch Match { get; set; }

        public override string ToString()
        {
            return Match.Header + Match.FlagName;
        }
    }
}
