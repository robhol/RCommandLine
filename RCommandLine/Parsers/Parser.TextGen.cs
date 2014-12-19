namespace RCommandLine.Parsers
{
    using System;
    using System.Linq;
    using System.Text;
    using Models;
    using Util;

    partial class Parser<TTarget>
    {

        /* Text generation */

        public string GetCommandList()
        {
            return GetCommandList(null);
        }

        string GetCommandList(Command startFrom)
        {
            var sb = new StringBuilder();
            startFrom = startFrom ?? RootCommand;

            Action<Command, string> walk = null;
            walk = (c, prefix) =>
            {
                if (!c.Hidden)
                    sb.Append(prefix).AppendLine(c.Name);

                foreach (var cc in c.Children)
                    walk(cc, prefix + c.Name + " ");
            };

            foreach (var command in startFrom.Children)
                walk(command, Options.BaseCommandName != null ? Options.BaseCommandName + " " : "");

            return sb.ToString();
        }

        void PrintCommandList()
        {
            Options.OutputTarget.WriteLine(string.Format("Available commands:\n{0}", GetCommandList(RootCommand)));
        }

        /// <summary>
        /// Gets a brief summary of command syntax, flags and arguments.
        /// </summary>
        /// <param name="shownCommand"></param>

        public string GetUsage(string shownCommand)
        {
            return GetUsage(RootCommand, shownCommand);
        }

        static string GetUsage(Command c, string shownCommand)
        {
            var parameters = c.Parameters
                .OrderByDescending(p => p is Flag)
                .ThenByDescending(f => f.Required)
                .ThenBy(f => (f is Argument) ? ((Argument) f).Order : 0).ToList();

            var usage = shownCommand + " " +
                        Util.JoinNotNulls(" ", parameters.Select(p => p.GetHelpTextRepresentation()));

            if (!string.IsNullOrEmpty(c.ExtraArgumentName))
                usage += " " + Parameter.GetHelpTextRepresentation(false, c.ExtraArgumentName);

            return usage;
        }

        public string GetUsageDescription()
        {
            return GetUsageDescription(RootCommand);
        }

        string GetUsageDescription(Command command)
        {
            var sb = new StringBuilder();

            var flags = command.Flags.ToList();

            var shortNameWidth = flags.Any() 
                ? Options.DefaultShortFlagHeader.Length + 2 
                : 0;

            var nameWidth = flags.Any() 
                ? Options.DefaultLongFlagHeader.Length + flags.Max(f => (!string.IsNullOrEmpty(f.Name) ? f.Name.Length : 0)) 
                : 0;

            foreach (var e in flags)
            {
                sb
                    .Append(e.Required ? "* " : "  ")
                    .Append((e.ShortName != default(char) ? (Options.DefaultShortFlagHeader + e.ShortName) : "").PadRight(shortNameWidth))
                    .Append((!string.IsNullOrEmpty(e.Name) ? (Options.DefaultLongFlagHeader + e.Name) : "").PadRight(nameWidth));

                if (e.Description != null)
                    sb
                        .Append(" - ")
                        .Append(e.Description);

                sb.AppendLine();
            }

            if (flags.Any())
                sb.AppendLine();

            var args = command.Arguments.ToList();

            nameWidth = args.Any() ? args.Max(a => a.Name.Length) : 0;
            foreach (var e in args)
            {
                sb
                    .Append(e.Required ? "* " : "  ")
                    .Append(e.Name.PadRight(nameWidth));

                if (e.Description != null)
                    sb
                        .Append(" - ")
                        .Append(e.Description);

                sb.AppendLine();
            }

            if (!string.IsNullOrEmpty(command.ExtraArgumentName))
            {
                sb
                    .Append("  ")
                    .Append(command.ExtraArgumentName.PadRight(nameWidth));

                if (!string.IsNullOrEmpty(command.ExtraArgumentDescription))
                    sb
                        .Append(" - ")
                        .Append(command.ExtraArgumentDescription);
            }

            if (command.UsageExamples.Any())
            {
                sb
                    .AppendLine()
                    .AppendLine("Example:");

                foreach (var ex in command.UsageExamples)
                {
                    sb
                        .Append("")
                        .AppendLine(ex.Usage)
                        .Append("  ")
                        .AppendLine(ex.Description);
                }
            }

            return sb.ToString();
        }

        private void PrintUsage(Command command, string displayedCommand)
        {
            Options.OutputTarget.WriteLine(string.Format("Usage:\n{0}\n{1}", 
                GetUsage(command, displayedCommand),
                GetUsageDescription(command)));
        }
    }
}