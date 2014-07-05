using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RCommandLine
{

    public class CommandParser<TTarget>
    {
        public string LastCommand { get; private set; }

        private readonly List<CommandElement> _commands;
        private readonly Type _topType;

        public CommandParser()
        {
            _commands = new List<CommandElement>();
            _topType = typeof(TTarget);

            ExploreCommandTree(_topType);
        }

        private void ExploreCommandTree(MemberInfo t, CommandElement parent = null)
        {
            foreach (var cmd in t.GetCustomAttributes<HasCommandAttribute>().Select(attr => new CommandElement(attr, parent)))
            {
                if (parent == null)
                    _commands.Add(cmd);

                ExploreCommandTree(cmd.CommandOptionsType, cmd);
            }
        }

        public object GetParser(IEnumerable<string> inputArgs, out Type parserType, out IEnumerable<string> remainingArgs, out string commandName)
        {
            var optType = _topType;

            CommandElement currentCommand = null;
            var currentCommandPath = new List<CommandElement>();

            var argQueue = new Queue<string>(inputArgs);

            while (argQueue.Count > 0)
            {
                var name = argQueue.Peek();


                var cmdSource = (currentCommand == null ? _commands : currentCommand.Children);
                var next =
                    cmdSource.FirstOrDefault(e => name.Equals(e.Name, StringComparison.InvariantCultureIgnoreCase));

                //no matching HasCommandAttribute property
                if (next == null)
                    break;

                currentCommand = next;
                currentCommandPath.Add(currentCommand);

                argQueue.Dequeue();
            }

            if (currentCommand != null)
                optType = currentCommand.CommandOptionsType;

            parserType = typeof(Parser<>).MakeGenericType(optType);
            remainingArgs = argQueue;
            commandName = string.Join(" ", currentCommandPath);

            return Activator.CreateInstance(parserType);
        }

        /// <summary>
        /// Parses (without joining strings) the given command.
        /// </summary>
        /// <returns>Returns the option object for the best matched HasCommandAttribute attribute.</returns>
        public CommandParseResult ParseIEnumerable(IEnumerable<string> inputArgs, out string commandName)
        {
            Type parserType;
            IEnumerable<string> remainingArgs;
            var parserObject = GetParser(inputArgs, out parserType, out remainingArgs, out commandName);

            var optObject = parserType.GetMethod("ParseIEnumerable").Invoke(parserObject, new object[] {remainingArgs});

            LastCommand = commandName;

            return new CommandParseResult(optObject, commandName);
        }

        

        /// <summary>
        /// Parses the selected string
        /// </summary>
        /// <param name="rawString">ArgumentAttribute string. Defaults to Environment.CommandLine</param>
        /// <returns></returns>
        public CommandParseResult Parse(string rawString)
        {
            string command;
            return Parse(rawString, out command);
        }

        /// <summary>
        /// Parses the selected string
        /// </summary>
        /// <param name="rawString">ArgumentAttribute string. Defaults to Environment.CommandLine</param>
        /// <param name="command">The final command, subcommands separated by space.</param>
        /// <returns></returns>
        public CommandParseResult Parse(string rawString, out string command)
        {
            return ParseIEnumerable(Util.JoinQuotedStringSegments((rawString ?? Environment.CommandLine).Split(' ')), out command);
        }



    }

}
