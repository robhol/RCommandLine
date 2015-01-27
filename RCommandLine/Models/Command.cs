namespace RCommandLine.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    class Command : Model
    {

        public List<Command> Children { get; private set; }
        private Command Parent { get; set; }

        public bool Hidden { get; set; }

        public Type OutputType { get; private set; }

        private readonly List<Flag> _flagList;
        public IEnumerable<Flag> Flags { get { return _flagList; } }

        private readonly List<Argument> _argumentList;
        public IEnumerable<Argument> Arguments { get { return _argumentList.OrderBy(a => a.Order); } }

        public IEnumerable<Parameter> Parameters { get { return Flags.Cast<Parameter>().Union(Arguments); } }

        public string ExtraArgumentName { get; set; }
        public string ExtraArgumentDescription { get; set; }

        private readonly List<CommandUsage> _usageExampleList;
        private readonly List<CommandRemark> _remarkList; 

        public IEnumerable<CommandUsage> UsageExamples
        {
            get { return _usageExampleList; }
        }

        public Command(Type encounteredInType, Type outputType, string name = null, bool hidden = false, Command parentCommand = null) : base(encounteredInType)
        {
            Children = new List<Command>();
            OutputType = outputType;
            Name = name;
            Hidden = hidden || outputType.IsAbstract;

            Parent = parentCommand;

            _flagList = new List<Flag>();
            _argumentList = new List<Argument>();
            _usageExampleList = new List<CommandUsage>();
            _remarkList = new List<CommandRemark>();
        }

        public void AddFlag(Flag f)
        {
            _flagList.Add(f);
        }

        public void AddArgument(Argument a)
        {
            _argumentList.Add(a);
        }

        public void AddUsageExample(CommandUsage commandUsage)
        {
            _usageExampleList.Add(commandUsage);
        }

        public void AddRemark(CommandRemark remark)
        {
            _remarkList.Add(remark);
        }

        public override string ToString()
        {
            return string.Format("{0}/{1}", Name, OutputType.Name);
        }

        public override string DefaultName
        {
            get { return Regex.Replace(OutputType.Name.ToLower(), "(options|command)?$", ""); }
        }

        public IEnumerable<CommandRemark> Remarks {
            get { return _remarkList; }
        }
    }
}
