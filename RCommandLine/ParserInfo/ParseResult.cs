using System;
using System.Collections.Generic;

namespace RCommandLine
{
    public class ParseResult
    {

        static readonly ParseResult NoResult = new ParseResult(null, null, null, null, null, false);
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

        public bool Success { get; private set; }

        private readonly ICommandParser _commandParser;
        private readonly IParameterParser _parameterParser;

        internal ParseResult(object finalOptions, string cmd, IList<string> extraArgs, ICommandParser commandParser, IParameterParser parameterParser, bool success)
        {
            Options = finalOptions;
            Command = cmd;
            ExtraArguments = extraArgs;
            Success = success;

            _commandParser = commandParser;
            _parameterParser = parameterParser;
        }



        public string GetCommandList()
        {
           return string.Format("Available commands:\n{0}", _commandParser.GetCommandList());
        }

        public void ShowCommandList()
        {
            Console.WriteLine(GetCommandList());
        }

        public string GetHelpText()
        {
            return string.Format("{0}\n\n{1}",
                        _parameterParser.GetUsage(string.IsNullOrEmpty(Command) ? "" : Command),
                        _parameterParser.GetArgumentList()
                        );
        }

        public void ShowHelpText()
        {
            Console.WriteLine(GetHelpText());
        }

    }

    

}
