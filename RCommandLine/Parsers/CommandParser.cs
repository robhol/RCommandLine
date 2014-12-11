/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RCommandLine
{

    public class CommandParser<TTarget> : ICommandParser
    {

        public ParserOptions ParserOptions { get; set; }

        /// <summary>
        /// Whether or not the command associated with this parent is childless.
        /// </summary>
        public bool IsTerminal { get { return _commands.Count == 0; } }

        private readonly List<Command> _commands;
        private readonly Type _topType;

        public CommandParser(ParserOptions options = null)
        {
            ParserOptions = options ?? new ParserOptions();

            _commands = new List<Command>();
            _topType = typeof(TTarget);
        }


        public string GetCommandList()
        {
            var sb = new StringBuilder();

            Action<Command, string> walk = null;
            walk = (c, prefix) =>
            {
                if (!c.Hidden)
                    sb.Append(prefix).AppendLine(c.Name);

                foreach (var cc in c.Children)
                    walk(cc, prefix + c.Name + " ");
            };

            foreach (var command in _commands)
                walk(command, ParserOptions.BaseCommandName != null ? ParserOptions.BaseCommandName + " " : "");

            return sb.ToString();
        }

    }

}
*/