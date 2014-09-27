using System.Collections.Generic;

namespace RCommandLine
{

    public interface IParameterParser
    {
        string GetUsage(string command);

        string GetArgumentList();

        int GetRequiredParameterCount();
    }

    public interface IParameterParser<out TTarget> : IParameterParser
    {
        TTarget ParseIEnumerable(IEnumerable<string> strEnumerable, out IEnumerable<string> extra);

        TTarget Parse(string str, out IEnumerable<string> extra);
    }
}
