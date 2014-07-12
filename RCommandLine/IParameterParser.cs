using System.Collections.Generic;

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
