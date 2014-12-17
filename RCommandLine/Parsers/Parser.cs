namespace RCommandLine.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Exceptions;
    using Models;
    using Parsing;
    using Util;

    public partial class Parser<TTarget> where TTarget : class
    {

        public ParserOptions Options { get; set; }

        internal Command RootCommand { get; private set; }

        internal Parser(ParserOptions options, Command cmd)
        {
            Options = options;
            RootCommand = cmd;
        }

        public ParseResult<TTarget> Parse(string argString, bool joinQuotedStrings = true)
        {
            return joinQuotedStrings 
                ? Parse(StringToArguments(argString)) 
                : Parse(argString.Split(' '));
        }

        public ParseResult<TTarget> Parse(IEnumerable<string> args = null)
        {
            args = args ?? Environment.GetCommandLineArgs().Skip(1);

            return Parse(args.Select(arg => new InputArgument {Value = arg}).ToList());
        }

        ParseResult<TTarget> Parse(IList<InputArgument> inputArguments)
        {

#if DEBUG
            if (inputArguments.Any() && inputArguments.Last().Value == "/!debug")
            {
                Console.WriteLine(">> Debug mode <<");
                Console.ReadKey(true);
                inputArguments.RemoveAt(inputArguments.Count - 1);
            }
#endif

            IEnumerable<InputArgument> commandArgs;
            string commandName;
            var command = GetCommand(inputArguments, out commandArgs, out commandName);

            var commandArgsQueue = new Queue<InputArgument>(commandArgs);

            var showList = (command == RootCommand || command.Hidden || command.OutputType.IsAbstract);
            var showUsage = !(command.Hidden || command.OutputType.IsAbstract);

            var displayedCommand =
                (string.IsNullOrEmpty(Options.BaseCommandName)
                    ? ""
                    : (Options.BaseCommandName + " " + commandName))
                + commandName;

            if ( (Options.AutomaticUsage && command.OutputType.IsAbstract) ||
                (Options.AutomaticUsage && !commandArgsQueue.Any() && command.Parameters.Any(p => p.Required)) ||
                (Options.AutomaticHelp && commandArgsQueue.Count == 1 && IsHelpFlagArgument(commandArgsQueue.Peek())))
            {
                if (showList)
                    PrintCommandList();

                if (showUsage)
                    PrintUsage(command, displayedCommand);

                return ErrorParseResult();
            }

            List<string> extraArguments = null;
            TTarget outputObject = null;

            try
            {
                outputObject = ParseArguments(command, commandArgsQueue, out extraArguments);
            }
            catch (MissingMethodException)
            {
                //Couldn't create output because it's abstract
                if (!Options.AutomaticUsage)
                    throw;

                return ErrorAndUsage(command, displayedCommand);
            }
            catch (UnrecognizedFlagException e)
            {
                if (!Options.AutomaticUsage)
                    throw;

                Console.WriteLine("The flag {0} was not recognized.", e.Flag);
                return ErrorAndUsage(command, displayedCommand);
            }
            catch (MissingValueException e)
            {
                if (!Options.AutomaticUsage)
                    throw;

                Console.WriteLine("Syntax error: the flag(s) {0} were not assigned a value.", string.Join(", ", e.Parameters));
                return ErrorParseResult();
            }
            catch (MissingArgumentException e)
            {
                if (!Options.AutomaticUsage)
                    throw;

                Console.WriteLine("Missing required argument(s) {0}", string.Join(", ", e.Parameters));
                return ErrorAndUsage(command, displayedCommand);
            }

            return new ParseResult<TTarget>(outputObject, commandName, extraArguments, this, true);
        }

        ParseResult<TTarget> ErrorAndUsage(Command cmd, string displayedCommand)
        {
            PrintUsage(cmd, displayedCommand);
            return ErrorParseResult();
        }

        bool IsHelpFlagArgument(InputArgument ia)
        {
            if (ia.Literal)
                return false;

            var helpFlag = FlagMatch.FromArgumentString(ia.Value, Options);
            return
                helpFlag != null
                && (helpFlag.MatchesFlag("help", FlagType.Long, false) || helpFlag.MatchesFlag("?", FlagType.Short, true));
        }

        List<InputArgument> StringToArguments(string s)
        {
            IEnumerable<bool> quotes;
            var segments = Util.JoinQuotedStringSegments(s.Split(' ').Where(seg => !string.IsNullOrEmpty(seg)), out quotes);
            return segments.Zip(quotes, (value, quoted) => new InputArgument {Value = value, Literal = quoted}).ToList();
        }

        Command GetCommand(IEnumerable<InputArgument> inputArgs, out IEnumerable<InputArgument> remainingArgs, out string commandName)
        {
            var currentCommand = RootCommand;
            var pathList = new List<Command>();

            var argQueue = new Queue<InputArgument>(inputArgs);

            while (argQueue.Any())
            {
                var name = argQueue.Peek();
                var nextCommand = currentCommand.Children.FirstOrDefault(e => name.Value.Equals(e.Name, StringComparison.InvariantCultureIgnoreCase));

                if (nextCommand == null)
                    break;

                currentCommand = nextCommand;
                pathList.Add(currentCommand);

                argQueue.Dequeue();
            }

            remainingArgs = argQueue;
            commandName = string.Join(" ", pathList.Select(c => c.Name));
            return currentCommand;
        }

        ParseResult<TTarget> ErrorParseResult()
        {
            return new ParseResult<TTarget>(null, null, null, this, false);
        }

        TTarget ParseArguments(Command cmd, Queue<InputArgument> inputArgs, out List<string> remaining)
        {

            var outputObject = (TTarget)Activator.CreateInstance(cmd.OutputType);
            
            foreach (var parameter in cmd.Parameters)
                parameter.ApplyDefaultValue(outputObject);

            var flagQueue = new Queue<Flag>();
            var argQueue = new Queue<Argument>(cmd.Arguments);

            var extraArguments = new List<string>();

            while (inputArgs.Any())
            {
                var discoveredFlags = new List<ParsedFlagArgumentInfo>();

                //Find any consecutive flags in the queue
                while (inputArgs.Any())
                {
                    var ia = inputArgs.Peek();

                    if (!MatchesFlagSyntax(ia))
                        break;

                    var flagInfos = ParseFlagArgument(cmd, ia.Value);

                    if (!flagInfos.Any()) 
                        break;

                    discoveredFlags.AddRange(flagInfos);
                    inputArgs.Dequeue();
                }

                //"pre-handle" any discovered flags
                foreach (var flagInfo in discoveredFlags)
                {
                    var flag = flagInfo.Element;

                    //check if we have a directly assigned value /flag:val
                    var val = flagInfo.Match.AssignmentValue;
                    if (!string.IsNullOrEmpty(val))
                    {
                        flag.PushValue(outputObject, flag.TargetType == typeof (bool)
                            ? ArgumentConverters.BooleanConverter(val)
                            : ArgumentConverters.DefaultConverter(val, flag.TargetType));
                    }
                    else //no directly assigned value
                    {
                        if (flag.TargetType == typeof(bool))
                            flag.PushValue(outputObject, true);
                        else
                            flagQueue.Enqueue(flag); //no value found - check later
                    }
                }

                //find any consecutive non-flags
                while (inputArgs.Any())
                {
                    var ia = inputArgs.Peek();

                    if (MatchesFlagSyntax(ia))
                        break;

                    //find a flag or arg to populate
                    if (flagQueue.Any())
                        flagQueue.Dequeue().ConvertAndPushValue(outputObject, ia.Value);
                    else
                    {
                        if (argQueue.Any())
                            argQueue.Dequeue().ConvertAndPushValue(outputObject, ia.Value);
                        else
                            extraArguments.Add(ia.Value);
                    }

                    inputArgs.Dequeue();
                }
            }

            var missing = cmd.Parameters.Where(parameter => parameter.Required && !parameter.HasValue).ToList();
            if (missing.Any())
                throw new MissingArgumentException("Missing required arguments.", missing.Select(p => p.Name));

            if (flagQueue.Any())
                throw new MissingValueException("Syntax error. Missing value for flag(s).", flagQueue.Select(f =>
                    f.Name != null
                    ? (Options.DefaultLongFlagHeader + f.Name) 
                    : (Options.DefaultShortFlagHeader + f.ShortName)));

            remaining = extraArguments;
            return outputObject;
        }

        bool MatchesFlagSyntax(InputArgument arg)
        {
            if (arg.Literal)
                return false;

            return Options.LongFlagHeaders
                .Union(Options.ShortFlagHeaders)
                .Any(arg.Value.StartsWith);
        }

        List<ParsedFlagArgumentInfo> ParseFlagArgument(Command cmd, string arg)
        {

            var info = new ParsedFlagArgumentInfo
            {
                Match = FlagMatch.FromArgumentString(arg, Options)
            };

            if (info.Match == null)
                return new List<ParsedFlagArgumentInfo>();

            var isLong = Options.LongFlagHeaders.Contains(info.Match.Header);
            if (isLong)
                info.Element = cmd.Flags.SingleOrDefault(f => f.Name.Equals(info.Match.FlagName, StringComparison.InvariantCultureIgnoreCase));
            if (info.Element != null)
                return new List<ParsedFlagArgumentInfo>(new[] {info});

            var isShort = Options.ShortFlagHeaders.Contains(info.Match.Header);
            if (!isShort)
                throw new UnrecognizedFlagException(info.Match.Header + info.Match.FlagName);

            var rv = new List<ParsedFlagArgumentInfo>();

            foreach (var c in info.Match.FlagName)
            {
                var element = cmd.Flags.SingleOrDefault(f => f.ShortName == c);

                if (element == null)
                    throw new UnrecognizedFlagException(string.Format("{0}{1}{2}",
                        info.Match.Header, info.Match.FlagName,
                        (isLong ? (" (" + info.Match.Header + info.Match.FlagName + ")") : "")));
                // if one char isn't found and the item is also valid as a long flag, also include the latter in case of user typo.

                rv.Add(new ParsedFlagArgumentInfo
                {
                    Element = element,
                    Match = info.Match.Clone(c.ToString(CultureInfo.InvariantCulture))
                });
            }

            if (string.IsNullOrEmpty(info.Match.AssignmentOperator)) return rv;

            if (rv.Count(fi => fi.Element.TargetType != typeof(bool)) > 1)
                throw new AmbiguousDirectAssignmentException(info.Match.Header + info.Match.FlagName,
                    "Can not directly assign more than one flag at a time.");

            return rv;
        }

        

    }


}
