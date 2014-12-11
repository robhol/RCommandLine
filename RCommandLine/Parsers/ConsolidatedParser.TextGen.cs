namespace RCommandLine
{
    using System;
    using System.Linq;
    using System.Text;

    partial class ConsolidatedParser<TTarget>
    {

        /* Text Generation */

        public string GetCommandList()
        {
            return GetCommandList(null);
        }

        string GetCommandList(Command startFrom)
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

            foreach (var command in RootCommand.Children)
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
            return shownCommand + " " +
                string.Join(" ",
                c.Parameters
                    .OrderBy(f => f.GetType().Name)
                    .ThenByDescending(f => f.Required)
                    .ThenBy(f => (f is Argument) ? ((Argument)f).Order : 0)
                    .Select(f => f.GetHelpTextRepresentation()));
        }

        public string GetArgumentList()
        {
            return GetArgumentList(RootCommand);
        }

        string GetArgumentList(Command command)
        {
            var sb = new StringBuilder();

            var flags = command.Flags.ToList();
            var nameWidth = flags.Any() ? flags.Max(f => (f.Name ?? "").Length) + 2 : 0;
            foreach (var e in flags)
            {
                sb
                    .Append("  ")
                    .Append((e.ShortName != default(char) ? (Options.DefaultShortFlagHeader + e.ShortName) : "").PadLeft(2))
                    .Append("  ")
                    .Append((e.Name != null ? (Options.DefaultLongFlagHeader + e.Name) : "").PadRight(nameWidth));

                if (e.Description != null)
                    sb
                        .Append(" - ")
                        .Append(e.Description);

                sb.AppendLine();
            }

            sb.AppendLine();

            nameWidth = command.Arguments.Any() ? command.Arguments.Max(a => a.Name.Length) : 0;
            foreach (var e in command.Arguments)
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

        private void PrintUsage(Command command, string displayedCommand)
        {
            Options.OutputTarget.WriteLine(string.Format("Usage:\n{0}\n{1}", 
                GetUsage(command, displayedCommand),
                GetArgumentList(command)));
        }
    }
}