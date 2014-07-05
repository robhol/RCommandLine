using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{
    public class Parser<TTarget> where TTarget : new()
    {

        List<FlagElement> _flags;
        List<ArgumentElement> _arguments;

        readonly Type _optionsType;

        public Parser()
        {
            _optionsType = typeof(TTarget);
            ExploreType();
        }

        /// <summary>
        /// Parses any string IEnumerable, but will not join strings.
        /// </summary>
        /// <param name="inputArgs"></param>
        /// <returns></returns>
        public TTarget ParseIEnumerable(IEnumerable<string> inputArgs)
        {
            var inputQueue = new Queue<string>(inputArgs);

            var targetObject = new TTarget();
            foreach (var e in _flags.Cast<ParameterElement>().Union(_arguments))
                e.Hydrate(targetObject);

            //Queues for "waiting" flags and parameters - ie. the next ones to receive a value.
            var flagQueue = new Queue<FlagElement>();
            var argQueue = new Queue<ArgumentElement>(_arguments.OrderBy(element => element.Order));
            
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
                        foreach (char c in arg.ToCharArray().Skip(1))
                        {
                            var flag = _flags.FirstOrDefault(f => f.ShortName == c);

                            if (flag == null)
                                throw new UnrecognizedFlagException(arg);

                            discovered.Add(flag);
                        }
                }

                //process new flags
                foreach (FlagElement flag in discovered)
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
                        if (argQueue.Count > 0)
                            argQueue.Dequeue().ConvertAndSetValue(targetObject, arg);
                    }

                }

            }

            var missingArgs =
                _arguments.Cast<ParameterElement>().Union(_flags).Where(a => a.Required && !a.HasValue).ToList();

            if (missingArgs.Any())
                throw new MissingValuesException(missingArgs);

            return targetObject;
        }

        /// <summary>
        /// Parses the selected string
        /// </summary>
        /// <param name="rawString">ArgumentAttribute string. Defaults to Environment.CommandLine</param>
        /// <returns></returns>
        public TTarget Parse(string rawString = null)
        {
            return ParseIEnumerable( Util.JoinQuotedStringSegments((rawString ?? Environment.CommandLine).Split(' ')) );
        }

        void ExploreType()
        {
            _flags = new List<FlagElement>();
            _arguments = new List<ArgumentElement>();

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
                    _arguments.Add(new ArgumentElement((ArgumentAttribute)attribute, prop, optionalInfo));
            }

        }

    }
}
