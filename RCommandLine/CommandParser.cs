using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RCommandLine
{

    public class CommandParser<TTarget> : ICommandParser<TTarget>
    {

        /// <summary>
        /// In help screens etc., show this command (usually the executable name) in front of all commands.
        /// </summary>
        public string BaseCommandName { get; set; }

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

        public IParameterParser<object> GetParser(IEnumerable<string> inputArgs, out Type parserType, out IEnumerable<string> remainingArgs, out string commandName)
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

            parserType = typeof(ParameterParser<>).MakeGenericType(optType);
            remainingArgs = argQueue;
            commandName = string.Join(" ", currentCommandPath.Select(c => c.Name));

            return (IParameterParser<object>)Activator.CreateInstance(parserType, commandName);
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
