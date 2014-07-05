using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{
    public class CommandParseResult
    {

        /// <summary>
        /// The ultimate options object
        /// </summary>
        public object Options { get; private set; }

        /// <summary>
        /// The complete command name, individual commands separated by space
        /// </summary>
        public string Command { get; private set; }

        public CommandParseResult(object finalOptions, string cmd)
        {
            Options = finalOptions;
            Command = cmd;
        }
        

    }
}
