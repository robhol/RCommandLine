using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{

    public interface ICommandParser
    {
        string GetCommandList();
    }

    interface ICommandParser<out TTarget> : ICommandParser
    {

        IParameterParser<object> GetParser(IEnumerable<string> inputArgs, out Type parserType,
            out IEnumerable<string> remainingArgs, out string commandName);

    }
}
