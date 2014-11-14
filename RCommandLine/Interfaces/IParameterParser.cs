using System.Collections.Generic;

namespace RCommandLine
{
    public interface IParameterParser<out TTarget>
    {
        /// <summary>
        /// Gets an automatically generated help text explaining all parameters.
        /// </summary>
        /// <param name="shownCommand"></param>
        /// <returns></returns>
        string GetUsage(string shownCommand);

        string GetArgumentList();

        int GetRequiredParameterCount();

        /// <summary>
        /// Whether or not this command has any children.
        /// </summary>
        bool IsTerminal { get; }

        TTarget ParseQueue(Queue<string> inputQueue, out IEnumerable<string> remaining, Queue<bool> stringQuoteStatus = null);

        TTarget Parse(string str, out IEnumerable<string> extra);
    }
}
