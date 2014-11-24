using System;
using System.Collections.Generic;

namespace RCommandLine
{

    public class ParseResult
    {

        /// <summary>
        /// The complete command name, individual commands separated by space
        /// </summary>
        public string Command { get; protected set; }

        /// <summary>
        /// Any user-supplied arguments that were not read as part of the usual parsing process
        /// </summary>
        public IList<string> ExtraArguments { get; protected set; }

        public bool Success { get; protected set; }

        protected readonly ICommandParser _commandParser;

        internal ParseResult(string cmd, IList<string> extraArgs, ICommandParser commandParser, bool success)
        {
            Command = cmd;
            ExtraArguments = extraArgs;
            Success = success;

            _commandParser = commandParser;
        }

    }

    public class ParseResult<TOptions> : ParseResult where TOptions : class
    {

        /// <summary>
        /// The ultimate options object
        /// </summary>
        public TOptions Options { get; private set; }

        
        private readonly IParameterParser<TOptions> _parameterParser;

        internal ParseResult(TOptions finalOptions, string cmd, IList<string> extraArgs, ICommandParser commandParser, IParameterParser<TOptions> parameterParser,
            bool success) : base(cmd, extraArgs, commandParser, success)
        {
            Options = finalOptions;
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
