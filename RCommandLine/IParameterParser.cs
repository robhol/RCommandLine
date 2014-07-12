using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{

    public interface IParameterParser
    {
        string GetUsage(string command);

        string GetArgumentList();
    }

    public interface IParameterParser<out TTarget> : IParameterParser
    {
        TTarget ParseIEnumerable(IEnumerable<string> strEnumerable);

        TTarget Parse(string str);

    }
}
