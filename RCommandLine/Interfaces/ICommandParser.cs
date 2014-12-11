using System;
using System.Collections.Generic;

namespace RCommandLine
{

    public interface ICommandParser
    {
        /// <summary>
        /// Generates a human-readable command list for this Command Parser.
        /// </summary>
        string GetCommandList();
    }
}
