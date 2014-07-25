using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RCommandLine
{

    public class ParameterParser<TTarget> : IParameterParser<TTarget> where TTarget : new()
    {

        List<FlagElement> _flags;
        List<OrderedParameterElement> _parameters;

        IEnumerable<CommonParameterElement> AllParameters { get { return _flags.Union<CommonParameterElement>(_parameters); } } 

        readonly Type _optionsType;

        public ParameterParser(string commandName = "")
        {
            _optionsType = typeof(TTarget);
            ExploreType();
        }

        /// <summary>
        /// Parses any string IEnumerable, but will not join strings.
        /// </summary>
        /// <returns>Options object</returns>
        public TTarget ParseIEnumerable(IEnumerable<string> inputArgs, out IEnumerable<string> remaining)
        {
            var inputQueue = new Queue<string>(inputArgs);
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

                while (inputQueue.Count > 0 && inputQueue.Peek().StartsWith("-"))
                {
                    var arg = inputQueue.Dequeue();
                    
                    if (arg.StartsWith("--"))
                    {
                        //get fully named flag
                        var flag = _flags.FirstOrDefault(f => f.Name != null && f.Name.Equals(arg.Substring(2), StringComparison.InvariantCultureIgnoreCase));

                        if (flag == null)
                            throw new UnrecognizedFlagException(arg);

                        discovered.Add(flag);
                    }
                    else
                        foreach (var flag in arg.ToCharArray().Skip(1).Select(c => _flags.FirstOrDefault(f => f.ShortName == c)))
                        {
                            if (flag == null)
                                throw new UnrecognizedFlagException(arg);

                            discovered.Add(flag);
                        }
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

                while (inputQueue.Count > 0 && !inputQueue.Peek().StartsWith("-"))
                {
                    //get all non-flags
                    var arg = inputQueue.Dequeue();

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
                throw new MissingArgumentException("Missing required arguments.", missing);

            if (flagQueue.Count > 0)
                throw new MissingValueException("Syntax error. Missing value for flag(s).", flagQueue);

            remaining = extraArguments;
            return targetObject;
        }

        /// <summary>
        /// Parses the selected string.
        /// </summary>
        /// <returns></returns>
        public TTarget Parse(string rawString, out IEnumerable<string> extra)
        {
            return ParseIEnumerable(Util.JoinQuotedStringSegments(rawString.Split(' ')), out extra);
        }

        public TTarget Parse(string rawString)
        {
            IEnumerable<string> _;
            return Parse(rawString, out _);
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
                    .Append((e.ShortName != default(char) ? ("-" + e.ShortName) : "").PadLeft(2))
                    .Append("  ")
                    .Append((e.Name != null ? ("--" + e.Name) : "").PadRight(nameWidth));

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
