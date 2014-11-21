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

        public string Header { get; set; }

        public string FlagName { get; set; }

        public string AssignmentOperator { get; set; }

        public string AssignmentValue { get; set; }

        public FlagElement Element { get; set; }

        public override string ToString()
        {
            return Header + FlagName;
        }
    }
}
