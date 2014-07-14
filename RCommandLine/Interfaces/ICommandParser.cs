using System;
using System.Collections.Generic;

namespace RCommandLine
{

    public interface ICommandParser
    {
        string GetCommandList();

        IParameterParser<object> GetParser(IEnumerable<string> inputArgs, out Type parserType,
            out IEnumerable<string> remainingArgs, out string commandName);

    }
}
