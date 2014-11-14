using System;
using System.Collections.Generic;

namespace RCommandLine
{

    public interface ICommandParser
    {
        /// <summary>
        /// Generates a human-readable command list for this Command Parser.
        /// </summary>
        string GetCommandList();

        /// <summary>
        /// Whether or not this command has any children.
        /// </summary>
        bool IsTerminal { get; }

        /// <summary>
        /// Gets a Parameter Parser equipped to handle flags and arguments.
        /// </summary>
        /// <param name="inputArgs">The args left over after parsing any commands</param>
        /// <param name="parserType">(out) the type of the parameter parser (ParameterParser&lt;T&gt;)</param>
        /// <param name="remainingArgs">(out) any args that were not used for named properties on the output Options object</param>
        /// <param name="commandName">(out) the full name of the command that was invoked</param>
        /// <returns></returns>
        IParameterParser<TOptions> GetParser<TOptions>(IEnumerable<string> inputArgs, out Type parserType,
            out IEnumerable<string> remainingArgs, out string commandName);

    }
}
