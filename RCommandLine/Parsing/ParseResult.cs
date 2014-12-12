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

        internal ParseResult(string cmd, IList<string> extraArgs, bool success)
        {
            Command = cmd;
            ExtraArguments = extraArgs;
            Success = success;
        }

    }

    public class ParseResult<TOptions> : ParseResult where TOptions : class, new()
    {
        private readonly Parser<TOptions> _parser;

        /// <summary>
        /// The ultimate options object
        /// </summary>
        public TOptions Options { get; private set; }

        internal ParseResult(TOptions finalOptions, string cmd, IList<string> extraArgs, Parser<TOptions> parser,
            bool success) : base(cmd, extraArgs, success)
        {
            _parser = parser;
            Options = finalOptions;
        }

        public string GetCommandList()
        {
            return string.Format("Available commands:\n{0}", _parser.GetCommandList());
        }

        public void ShowCommandList()
        {
            Console.WriteLine(GetCommandList());
        }

        public string GetHelpText()
        {
            return string.Format("{0}\n\n{1}",
                _parser.GetUsage(string.IsNullOrEmpty(Command) ? "" : Command),
                _parser.GetUsageDescription()
                );
        }

        public void ShowHelpText()
        {
            Console.WriteLine(GetHelpText());
        }

    }



}
