using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{
    public class ParserOptions
    {
        private List<string> _flagValueSeparators;
        private List<string> _longFlagHeaders;
        private List<string> _shortFlagHeaders;

        public enum Template
        {
            /// <summary>
            /// Accepts GNU/Unix-style as well as DOS/Windows-style arguments
            /// </summary>
            Default,
            /// <summary>
            /// Accepts GNU/Unix-style arguments (--flag, -f), no joined assignment (-f 3 ok, -f:3 error)
            /// </summary>
            Unix,
            /// <summary>
            /// Accepts DOS/Windows-style arguments (/flag, /f), accepts joined assignment (/f 3 = /f=3 or /f:3) 
            /// </summary>
            Windows
        }

        private static Dictionary<Template, List<string>> _defaultFlagValueSeparators = new Dictionary<Template, List<string>>
        {
            {Template.Default,  new List<string>{"=", ":"}},
            {Template.Unix,     new List<string>()},
            {Template.Windows,  new List<string>{"=", ":"}},
        };

        private static Dictionary<Template, List<string>> _defaultLongFlagHeaders = new Dictionary<Template, List<string>>
        {
            {Template.Default,  new List<string>{"--", "/"}},
            {Template.Unix,     new List<string>{"--"}},
            {Template.Windows,  new List<string>{"/"}},
        };

        private static Dictionary<Template, List<string>> _defaultShortFlagHeaders = new Dictionary<Template, List<string>>
        {
            {Template.Default,  new List<string>{"-", "/"}},
            {Template.Unix,     new List<string>{"-"}},
            {Template.Windows,  new List<string>{"/"}},
        };

        public ParserOptions(Template optionsTemplate = Template.Default, string baseCommandName = null)
        {
            AutomaticCommandList = true;
            AutomaticUsage = true;
            AutomaticHelp = true;

            BaseCommandName     = baseCommandName;
            FlagValueSeparators = _defaultFlagValueSeparators[optionsTemplate];
            LongFlagHeaders     = _defaultLongFlagHeaders[optionsTemplate];
            ShortFlagHeaders    = _defaultShortFlagHeaders[optionsTemplate];
        }

        /// <summary>
        /// Whether or not to show a list of acceptable commands when input is empty.
        /// </summary>
        public bool AutomaticCommandList { get; set; }

        /// <summary>
        /// Whether or not to display a command's usage when given arguments are insufficient or incorrect
        /// </summary>
        public bool AutomaticUsage { get; set; }

        /// <summary>
        /// Whether or not to intercept -? and --help.
        /// </summary>
        public bool AutomaticHelp { get; set; }

        /// <summary>
        /// In help screens etc., show this command (usually the executable name) in front of all commands.
        /// </summary>
        public string BaseCommandName { get; set; }

        /// <summary>
        /// Specifies the possible strings that may separate a flag from its value.
        /// Note that space is mandatory (and separate from this list).
        /// </summary>
        public List<string> FlagValueSeparators
        {
            get { return _flagValueSeparators; }
            set { _flagValueSeparators = value.OrderByDescending(f => f.Length).ThenBy(f => f).ToList(); }
        }

        /// <summary>
        /// Specifies the possible strings that may denote a "fully named" flag (--flag)
        /// </summary>
        public List<string> LongFlagHeaders
        {
            get { return _longFlagHeaders; }
            set { _longFlagHeaders = value.OrderByDescending(f => f.Length).ThenBy(f => f).ToList(); }
        }

        private string _defaultLongFlagHeader;
        /// <summary>
        /// Specifies the long flag header to be used in automatically generated text and examples.
        /// Defaults to the first header specified.
        /// </summary>
        public string DefaultLongFlagHeader
        {
            get { return _defaultLongFlagHeader ?? LongFlagHeaders.First(); }
            set { _defaultLongFlagHeader = value; }
        }

        /// <summary>
        /// Specifies the possible strings that may denote a "short" flag (-F)
        /// </summary>
        public List<string> ShortFlagHeaders
        {
            get { return _shortFlagHeaders; }
            set { _shortFlagHeaders = value.OrderByDescending(f => f.Length).ThenBy(f => f).ToList(); }
        }

        private string _defaultShortFlagHeader;
        /// <summary>
        /// Specifies the short flag header to be used in automatically generated text and examples.
        /// Defaults to the first header specified.
        /// </summary>
        public string DefaultShortFlagHeader
        {
            get { return _defaultShortFlagHeader ?? ShortFlagHeaders.First(); }
            set { _defaultShortFlagHeader = value; }
        }
    }
}
