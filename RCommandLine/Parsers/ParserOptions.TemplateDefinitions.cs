namespace RCommandLine.Parsers
{
    using System.Collections.Generic;

    partial class ParserOptions
    {

        private static readonly ParserOptionsTemplates TemplatesObject = new ParserOptionsTemplates();
        public static ParserOptionsTemplates Templates { get { return TemplatesObject; }}

        public class ParserOptionsTemplates
        {

            internal ParserOptionsTemplates()
            {
                Default = new ParserOptions();

                Unix = new ParserOptions
                {
                    FlagValueSeparators = new List<string>(),
                    LongFlagHeaders = new List<string> { "--" },
                    ShortFlagHeaders = new List<string> { "-" }
                };

                Windows = new ParserOptions
                {
                    FlagValueSeparators = new List<string>{"=", ":"},
                    LongFlagHeaders = new List<string> { "/" },
                    ShortFlagHeaders = new List<string> { "/" }
                };
            }

            /// <summary>
            /// Accepts GNU/Unix-style as well as DOS/Windows-style arguments
            /// </summary>
            public ParserOptions Default { get; private set; }
            /// <summary>
            /// Accepts GNU/Unix-style arguments (--flag, -f), no joined assignment (-f 3 ok, -f:3 error)
            /// </summary>
            public ParserOptions Unix { get; private set; }
            /// <summary>
            /// Accepts DOS/Windows-style arguments (/flag, /f), accepts joined assignment (/f 3 = /f=3 or /f:3) 
            /// </summary>
            public ParserOptions Windows { get; private set; }
        }

    }
}
