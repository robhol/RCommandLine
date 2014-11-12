using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RCommandLine
{

    public class ParameterParser<TTarget> : IParameterParser<TTarget> where TTarget : new()
    {

        public ParserOptions ParserOptions { get; set; }
            
        List<FlagElement> _flags;
        List<OrderedParameterElement> _parameters;

        IEnumerable<CommonParameterElement> AllParameters { get { return _flags.Union<CommonParameterElement>(_parameters); } } 

        readonly Type _optionsType;

        public ParameterParser(ParserOptions options = null, bool isTerminal = true)
        {
            ParserOptions = options ?? new ParserOptions();
            _optionsType = typeof(TTarget);
            IsTerminal = isTerminal;
            ExploreType();
        }

        /// <summary>
        /// Parses a Queue with input 
        /// </summary>
        /// <returns>Options object</returns>
        public TTarget ParseQueue(Queue<string> inputQueue, out IEnumerable<string> remaining, Queue<bool> stringQuoteStatus = null)
        {
            stringQuoteStatus = stringQuoteStatus ?? new Queue<bool>(inputQueue.Select(_ => false));

            var targetObject = new TTarget();
            foreach (var e in AllParameters)
                e.Hydrate(targetObject);

            //Queues for "waiting" flags and parameters - ie. the next ones to receive a value.
            var flagQueue = new Queue<FlagElement>();
            var parameterQueue = new Queue<OrderedParameterElement>(_parameters.OrderBy(element => element.Order));

            var extraArguments = new List<string>();
            
            while (inputQueue.Count > 0)
            {
                //get flag(s)
                var discovered = new List<FlagElement>();

                //if input queue is not empty and next is a string
                while (inputQueue.Count > 0 && 
                    (!stringQuoteStatus.Peek() && IsFlag(inputQueue.Peek())))
                {
                    var arg = inputQueue.Dequeue();
                    stringQuoteStatus.Dequeue();

                    //try to match as either kind of flag string
                    var longFlagString = GetFlagName(arg, true, false);
                    var shortFlagString = GetFlagName(arg, false, true);

                    FlagElement flag;

                    if (longFlagString != null)
                    {
                        flag =
                            _flags.FirstOrDefault(
                                f =>
                                    f.Name != null &&
                                    f.Name.Equals(longFlagString, StringComparison.InvariantCultureIgnoreCase));

                        if (flag != null)
                        {
                            discovered.Add(flag);
                            continue;
                        }
                    }

                    var shortFlagsFail = false;
                    if (shortFlagString != null)
                        foreach (var sfc in shortFlagString)
                        {
                            var fe = _flags.SingleOrDefault(f => f.ShortName == sfc);

                            if (fe == null)
                            {
                                shortFlagsFail = true;
                                break;
                            }

                            discovered.Add(fe);
                        }
                            
                    if (shortFlagsFail || !discovered.Any())
                        throw new UnrecognizedFlagException(ParserOptions.DefaultLongFlagHeader + longFlagString);

                }

                //process new flags
                foreach (var flag in discovered)
                {
                    if (flag.TargetType == typeof(bool))
                    {
                        //boolean flags don't get values, they're just marked as present and forgotten
                        flag.SetValue(targetObject, true);
                    }
                    else
                        flagQueue.Enqueue(flag);
                }

                while (inputQueue.Count > 0 &&
                    (stringQuoteStatus.Peek() || !IsFlag(inputQueue.Peek())))
                {

                    //get all non-flags
                    var arg = inputQueue.Dequeue();
                    stringQuoteStatus.Dequeue();

                    if (flagQueue.Count > 0)
                    {
                        //attempt to assign value to first flag in line
                        flagQueue.Dequeue().ConvertAndSetValue(targetObject, arg);
                    }
                    else
                    {
                        if (parameterQueue.Count > 0)
                            parameterQueue.Dequeue().ConvertAndSetValue(targetObject, arg);
                        else
                            extraArguments.Add(arg);
                    }
                }
            }

            var missing = AllParameters.Where(a => a.Required && !a.HasValue).ToList();

            if (missing.Any())
                throw new MissingArgumentException("Missing required arguments.", missing.Select(a => a.Name));

            if (flagQueue.Count > 0)
                throw new MissingValueException("Syntax error. Missing value for flag(s).", flagQueue.Select(f => 
                    f.Name != null ? 
                    (ParserOptions.DefaultLongFlagHeader + f.Name) : 
                    (ParserOptions.DefaultShortFlagHeader + f.ShortName)));

            remaining = extraArguments;
            return targetObject;
        }

        /// <summary>
        /// Parses the provided string.
        /// </summary>
        public TTarget Parse(string rawString, out IEnumerable<string> extra)
        {
            Queue<bool> quoteInfo;
            var queue = new Queue<string>(Util.JoinQuotedStringSegments(rawString.Split(' '), out quoteInfo));
            return ParseQueue(queue, out extra, quoteInfo);
        }

        public TTarget Parse(string rawString)
        {
            IEnumerable<string> _;
            return Parse(rawString, out _);
        }

        bool IsFlag(string f)
        {
            return ParserOptions.LongFlagHeaders
                .Union(ParserOptions.ShortFlagHeaders)
                .Any(f.StartsWith);
        }

        string GetFlagName(string f, bool checkLong, bool checkShort)
        {
            var match = Enumerable.Empty<string>()
                .Union(checkLong  ? ParserOptions.LongFlagHeaders  : Enumerable.Empty<string>())
                .Union(checkShort ? ParserOptions.ShortFlagHeaders : Enumerable.Empty<string>())
                .FirstOrDefault(f.StartsWith);

            return match != null ? f.Substring(match.Length) : null;
        }

        /// <summary>
        /// Gets a brief summary of command syntax, flags and arguments.
        /// </summary>
        /// <param name="shownCommand"></param>
        /// <returns></returns>
        public string GetUsage(string shownCommand)
        {
            return shownCommand + " " +
                string.Join(" ", 
                AllParameters
                    .OrderBy(f => f.GetType().Name)
                    .ThenByDescending(f => f.Required)
                    .ThenBy(f => (f is OrderedParameterElement) ? ((OrderedParameterElement)f).Order : 0)
                    .Select(f => f.GetHelpTextRepresentation() ));
        }

        public string GetArgumentList()
        {
            var sb = new StringBuilder();

            var nameWidth = _flags.Count > 0 ? _flags.Max(f => (f.Name ?? "").Length) + 2 : 0;
            foreach (var e in _flags)
            {
                sb
                    .Append("  ")
                    .Append((e.ShortName != default(char) ? (ParserOptions.DefaultShortFlagHeader + e.ShortName) : "").PadLeft(2))
                    .Append("  ")
                    .Append((e.Name != null ? (ParserOptions.DefaultLongFlagHeader + e.Name) : "").PadRight(nameWidth));

                if (e.Description != null)
                    sb
                        .Append(" - ")
                        .Append(e.Description);

                sb.AppendLine();
            }

            sb.AppendLine();

            nameWidth = _parameters.Count > 0 ? _parameters.Max(a => a.Name.Length) : 0;
            foreach (var e in _parameters)
            {
                sb
                    .Append(e.Name.PadRight(nameWidth));

                if (e.Description != null)
                    sb
                        .Append(" - ")
                        .Append(e.Description);

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public int GetRequiredParameterCount()
        {
            return AllParameters.Count(p => p.Required);
        }

        /// <summary>
        /// Whether the options object belonging to this ParameterParser lacks subcommands
        /// </summary>
        public bool IsTerminal { get; private set; }

        void ExploreType()
        {
            _flags = new List<FlagElement>();
            _parameters = new List<OrderedParameterElement>();

            foreach (var prop in _optionsType.GetProperties())
            {
                var attribute = prop.GetCustomAttribute<ElementAttribute>();

                if (attribute == null)
                    continue;

                var optionalInfo = prop.GetCustomAttribute<OptionalAttribute>();

                var flag = attribute as FlagAttribute;
                if (flag != null)
                    _flags.Add(new FlagElement(flag, prop, optionalInfo));
                else
                    _parameters.Add(new OrderedParameterElement((OrderedParameterAttribute)attribute, prop, optionalInfo));
            }

        }

    }
}
