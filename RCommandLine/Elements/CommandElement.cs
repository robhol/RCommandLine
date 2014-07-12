using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RCommandLine
{
    /// <summary>
    /// Internal representation of a HasCommandAttribute
    /// </summary>
    class CommandElement : Element
    {

        readonly List<CommandElement> _children = new List<CommandElement>();
        public IEnumerable<CommandElement> Children { get { return _children; } }
        public CommandElement Parent { get; private set; }

        public bool Hidden { get; private set; }

        public Type CommandOptionsType { get; private set; }

        public CommandElement(HasCommandAttribute cmd, CommandElement parentCommand = null)
        {
            CommandOptionsType = cmd.CommandOptionsType;
            Name = cmd.Name ?? Regex.Replace(CommandOptionsType.Name.ToLower(), "options?$", "");
            Hidden = cmd.Hidden;

            Parent = parentCommand;
            
            if (Parent != null)
                Parent._children.Add(this);
        }

    }
}
