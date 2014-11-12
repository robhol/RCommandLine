using System.Collections.Generic;

namespace RCommandLine
{

    public interface IParameterParser
    {
        string GetUsage(string command);

        string GetArgumentList();

        int GetRequiredParameterCount();

        /// <summary>
        /// Whether or not this command has any children.
        /// </summary>
        bool IsTerminal { get; }
    }

    public interface IParameterParser<out TTarget> : IParameterParser
    {
        TTarget ParseQueue(Queue<string> inputQueue, out IEnumerable<string> remaining, Queue<bool> stringQuoteStatus = null);
        TTarget Parse(string str, out IEnumerable<string> extra);
    }
}
