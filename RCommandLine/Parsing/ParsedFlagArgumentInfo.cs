﻿namespace RCommandLine.Parsing
{
    using Models;

    /// <summary>
    /// Temporary information object used in parsing
    /// </summary>
    class ParsedFlagArgumentInfo
    {
        public Flag Element { get; set; }

        public FlagMatch Match { get; set; }

        public override string ToString()
        {
            return Match.Header + Match.FlagName;
        }
    }
}
