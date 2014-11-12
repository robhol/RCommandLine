using System;
using System.Collections.Generic;
using System.Linq;
using RCommandLine.Output;

namespace RCommandLine
{
    public class Parser<TTarget>
    {

        public ParserOptions Options { get; set; }

        private ICommandParser _commandParser;
        private IParameterParser _parameterParser;

        private string _commandName;

        /// <summary>
        /// The target to which output should be written.
        /// </summary>
        public IOutputTarget OutputTarget { get; set; }

        public Parser(ParserOptions options = null, string baseCommandName = null)
        {
            Options = options ?? new ParserOptions(baseCommandName);
            OutputTarget = ConsoleOutputChannel.Instance;
        }

        /// <summary>
        /// Parses the input arguments.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="joinStringSegments">If true, quoted string segments containing spaces will be joined before parsing.</param>
        /// <returns></returns>
        public ParseResult Parse(IEnumerable<string> args, bool joinStringSegments = true)
        {

            _commandParser = new CommandParser<TTarget>(Options);

            var argList = args.ToList();

#if DEBUG
            if (argList.LastOrDefault() == "/!debug")
            {
                Console.WriteLine(">> Debug mode <<");
                Console.ReadKey(true);
                argList.RemoveAt(argList.Count - 1);
            }
#endif

            if (Options.AutomaticCommandList && string.IsNullOrEmpty(argList.FirstOrDefault()) && !_commandParser.IsTerminal )
            {
                PrintCommandList();
                return ErrorParseResult();
            }

            IEnumerable<string> remainingArgs;
            try
            {
                Type parserType;
                _parameterParser = _commandParser.GetParser(argList, out parserType, out remainingArgs, out _commandName);
                
            }
            catch (ArgumentException) //option type has no default constructor
            {
                PrintCommandList();
                return ErrorParseResult();
            }
            
            var remainingArgsList = remainingArgs.ToList();

            //TODO use GetFlagName
            if (Options.AutomaticHelp && remainingArgsList.Count == 1 && new[] { "-?", "--help" }.Contains(remainingArgsList.First().ToLower()))
            {

                if (_parameterParser == null || string.IsNullOrWhiteSpace(_commandName))
                    if (!_commandParser.IsTerminal)
                        PrintCommandList();
                    else
                        PrintHelpScreen();

                return new ParseResult(null, _commandName, null, _commandParser, _parameterParser, false);
            }

            try
            {
                var pparser = (IParameterParser<object>) _parameterParser;
                IEnumerable<string> extra;
                object options;
                if (joinStringSegments)
                {
                    Queue<bool> stringQuoteInfo;
                    var x = Util.JoinQuotedStringSegments(remainingArgsList, out stringQuoteInfo);
                    options = pparser.ParseQueue(new Queue<string>(x), out extra, stringQuoteInfo);
                }
                else
                    options = pparser.ParseQueue(new Queue<string>(remainingArgsList), out extra);

                return new ParseResult(options, _commandName, extra.ToList(), _commandParser, _parameterParser, true);
            }
            catch (UnrecognizedFlagException e)
            {
                if (!Options.AutomaticUsage)
                    throw;

                ErrorAndUsage(string.Format("The flag {0} was not recognized.", e.Flag), _commandName, _parameterParser);
                return ErrorParseResult();
            }
            catch (MissingArgumentException e)
            {
                if (!Options.AutomaticUsage)
                    throw;

                ErrorAndUsage(string.Format(e.Message + " " + string.Join(", ", e.Parameters)), _commandName, _parameterParser);
                return ErrorParseResult();
            }
            catch (MissingValueException e)
            {
                ErrorAndUsage("Missing values for " + string.Join(", ", e.Parameters), _commandName, _parameterParser);
                return ErrorParseResult();
            }

        }

        void PrintCommandList()
        {
            OutputTarget.WriteLine(string.Format("Available commands:\n{0}", _commandParser.GetCommandList()));
        }

        void PrintHelpScreen()
        {
            OutputTarget.WriteLine(string.Format("{0}{1}\n\n{2}",
                (Options.BaseCommandName == null ? "" : Options.BaseCommandName + " "),
                _commandName,
                _parameterParser.GetArgumentList()
                ));
        }

        void ErrorAndUsage(string err, string cmd, IParameterParser p)
        {
            OutputTarget.WriteLine(err + Environment.NewLine + "Usage for command " + cmd);
            OutputTarget.WriteLine(p.GetUsage(cmd));
        }

        ParseResult ErrorParseResult()
        {
            return new ParseResult(null, _commandName, null, _commandParser, _parameterParser, false);
        }

        public ParseResult Parse(string args = null)
        {
            return Parse((args != null ? args.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) : Environment.GetCommandLineArgs().Skip(1)));
        }

    }
}
