namespace RCommandLine
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    class FlagMatch
    {
        private readonly ParserOptions _options;
        public string Header { get; set; }
        public string FlagName { get; set; }
        public string AssignmentOperator { get; set; }
        public string AssignmentValue { get; set; }

        public FlagType Type { get; private set; }

        private FlagMatch(ParserOptions options)
        {
            _options = options;
            Type = FlagType.None;
        }

        private FlagMatch(Match match, ParserOptions options) : this(options)
        {
            Header = match.Groups[1].Value;
            FlagName = match.Groups[2].Value;
            AssignmentOperator = match.Groups.Count >= 5 ? match.Groups[4].Value : null;
            AssignmentValue = match.Groups.Count >= 6 ? match.Groups[5].Value : null;

            Type |= options.ShortFlagHeaders.Contains(Header) ? FlagType.Short : 0;
            Type |= options.LongFlagHeaders.Contains(Header)  ? FlagType.Long  : 0;
        }

        public static FlagMatch FromArgumentString(string arg, ParserOptions options)
        {
            var match = Regex.Match(arg, string.Format("{0}(.+?)({1}(.*))?$",
                Util.RegexForAnyLiteral(options.LongFlagHeaders.Union(options.ShortFlagHeaders)),
                Util.RegexForAnyLiteral(options.FlagValueSeparators)
                ));

            return match.Success ? new FlagMatch(match, options) : null;
        }

        public FlagMatch Clone(string newName = null)
        {
            return new FlagMatch(_options)
            {
                Header = Header,
                FlagName = newName ?? FlagName,
                AssignmentOperator = AssignmentOperator,
                AssignmentValue = AssignmentValue
            };
        }

        public bool MatchesFlag(string name, FlagType type, bool caseSensitive)
        {
            return Type.HasFlag(type) && FlagName.Equals(name, caseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase);
        }

    }
}