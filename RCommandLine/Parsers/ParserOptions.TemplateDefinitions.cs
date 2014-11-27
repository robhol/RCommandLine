using System.Collections.Generic;

namespace RCommandLine
{
    partial class ParserOptions
    {
        private static readonly Dictionary<Template, List<string>> DefaultFlagValueSeparators = new Dictionary<Template, List<string>>
        {
            {Template.Default,  new List<string>{"=", ":"}},
            {Template.Unix,     new List<string>()},
            {Template.Windows,  new List<string>{"=", ":"}},
        };

        private static readonly Dictionary<Template, List<string>> DefaultLongFlagHeaders = new Dictionary<Template, List<string>>
        {
            {Template.Default,  new List<string>{"--", "/"}},
            {Template.Unix,     new List<string>{"--"}},
            {Template.Windows,  new List<string>{"/"}},
        };

        private static readonly Dictionary<Template, List<string>> DefaultShortFlagHeaders = new Dictionary<Template, List<string>>
        {
            {Template.Default,  new List<string>{"-", "/"}},
            {Template.Unix,     new List<string>{"-"}},
            {Template.Windows,  new List<string>{"/"}},
        };

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

    }
}
