using System;
using System.Collections.Generic;

namespace RCommandLine
{
    public class ParseResult
    {

        static readonly ParseResult NoResult = new ParseResult(null, null, null, null, null);
        public static ParseResult None { get { return NoResult; } }

        /// <summary>
        /// The ultimate options object
        /// </summary>
        public object Options { get; private set; }

        /// <summary>
        /// The complete command name, individual commands separated by space
        /// </summary>
        public string Command { get; private set; }

        public IList<string> ExtraArguments { get; private set; }

        private readonly ICommandParser _commandParser;
        private readonly IParameterParser _parameterParser;

        internal ParseResult(object finalOptions, string cmd, IList<string> extraArgs, ICommandParser commandParser, IParameterParser parameterParser)
        {
            Options = finalOptions;
            Command = cmd;
            ExtraArguments = extraArgs;

            _commandParser = commandParser;
            _parameterParser = parameterParser;
        }

        public void ShowCommandList()
        {
            Console.WriteLine("Available commands: " + Environment.NewLine + _commandParser.GetCommandList());
        }

        public void ShowHelpText()
        {
            Console.WriteLine(
                        _parameterParser.GetUsage(string.IsNullOrEmpty(Command) ? "" : Command) + Environment.NewLine + Environment.NewLine +
                        _parameterParser.GetArgumentList()
                        );
        }

    }

    

}
