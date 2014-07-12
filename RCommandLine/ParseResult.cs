using System;

namespace RCommandLine
{
    public class ParseResult
    {

        static readonly ParseResult none = new ParseResult(null, null, null, null);
        public static ParseResult None { get { return none; } }

        /// <summary>
        /// The ultimate options object
        /// </summary>
        public object Options { get; private set; }

        /// <summary>
        /// The complete command name, individual commands separated by space
        /// </summary>
        public string Command { get; private set; }

        private readonly ICommandParser _commandParser;
        private readonly IParameterParser _parameterParser;

        public ParseResult(object finalOptions, string cmd, ICommandParser commandParser, IParameterParser parameterParser)
        {
            Options = finalOptions;
            Command = cmd;

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
