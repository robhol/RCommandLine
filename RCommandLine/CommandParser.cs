using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RCommandLine
{

    public class CommandParser<TTarget> : ICommandParser
    {

        /// <summary>
        /// In help screens etc., show this command (usually the executable name) in front of all commands.
        /// </summary>
        public string BaseCommandName { get; set; }

        public bool IsTerminal { get { return _commands.Count == 0; } }

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
            foreach (var cmd in t.GetCustomAttributes<HasCommandAttribute>(false).Select(attr => new CommandElement(attr, parent)))
            {
                if (parent == null)
                    _commands.Add(cmd);

                ExploreCommandTree(cmd.CommandOptionsType, cmd);
            }
        }

        public IParameterParser<object> GetParser(IEnumerable<string> inputArgs, out Type parserType, out IEnumerable<string> remainingArgs, out string commandName)
        {
            var optType = _topType;

            CommandElement currentCommand = null;
            var commandPathList = new List<CommandElement>();

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
                commandPathList.Add(currentCommand);

                argQueue.Dequeue();
            }

            if (currentCommand != null)
                optType = currentCommand.CommandOptionsType;

            //will throw ArgumentException if the optType has no default constructor
            parserType = typeof(ParameterParser<>).MakeGenericType(optType);
            var isTerminal = !optType.GetCustomAttributes<HasCommandAttribute>().Any();

            remainingArgs = argQueue;
            commandName = string.Join(" ", commandPathList.Select(c => c.Name));

            return (IParameterParser<object>) Activator.CreateInstance(parserType, commandName, isTerminal);
        }

        public string GetCommandList()
        {
            var sb = new StringBuilder();

            Action<CommandElement, string> walk = null;
            walk = (c, prefix) =>
            {
                if (!c.Hidden)
                    sb.Append(prefix).AppendLine(c.Name);

                foreach (var cc in c.Children)
                    walk(cc, prefix + c.Name + " ");
            };

            foreach (var command in _commands)
                walk(command, BaseCommandName != null ? BaseCommandName + " " : "");

            return sb.ToString();
        }

    }

}
