using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{
    public class ParserOptions
    {

        public ParserOptions(string baseCommandName = null)
        {
            AutomaticCommandList = true;
            AutomaticUsage = true;
            AutomaticHelp = true;

            BaseCommandName = baseCommandName;
        }

        /// <summary>
        /// Whether or not to show a list of acceptable commands when input is empty.
        /// </summary>
        public bool AutomaticCommandList { get; set; }

        /// <summary>
        /// Whether or not to display a command's usage when given arguments are insufficient or incorrect
        /// </summary>
        public bool AutomaticUsage { get; set; }

        /// <summary>
        /// Whether or not to intercept -? and --help.
        /// </summary>
        public bool AutomaticHelp { get; set; }

        /// <summary>
        /// In help screens etc., show this command (usually the executable name) in front of all commands.
        /// </summary>
        public string BaseCommandName { get; set; }


    }
}
