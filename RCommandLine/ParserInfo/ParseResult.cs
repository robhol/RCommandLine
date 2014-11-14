using System;
using System.Collections.Generic;

namespace RCommandLine
{
    public class ParseResult<TOptions> where TOptions : class
    {

        /// <summary>
        /// The ultimate options object
        /// </summary>
        public TOptions Options { get; private set; }

        /// <summary>
        /// The complete command name, individual commands separated by space
        /// </summary>
        public string Command { get; private set; }

        /// <summary>
        /// Any user-supplied arguments that were not read as part of the usual parsing process
        /// </summary>
        public IList<string> ExtraArguments { get; private set; }

        public bool Success { get; private set; }

        private readonly ICommandParser _commandParser;
        private readonly IParameterParser<TOptions> _parameterParser;

        internal ParseResult(TOptions finalOptions, string cmd, IList<string> extraArgs, ICommandParser commandParser, IParameterParser<TOptions> parameterParser,
            bool success)
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
