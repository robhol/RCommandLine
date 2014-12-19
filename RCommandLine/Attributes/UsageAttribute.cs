using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine.Attributes
{
    /// <summary>
    /// Provides a human-readable example invocation of this command with an optional description.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class UsageAttribute : Attribute
    {

        private readonly int _order;
        private readonly string _usage;
        private readonly string _description;

        public int Order
        {
            get { return _order; }
        }

        public string Usage
        {
            get { return _usage; }
        }

        public string Description
        {
            get { return _description; }
        }

        /// <param name="usage">An example invocation of your program, excluding the command part (provided automatically).</param>
        /// <param name="description">Optional - tell the user what this particular example will do.</param>
        public UsageAttribute(string usage, string description = null) : this(0, usage, description) { }

        /// <param name="order">Attributes are not guaranteed to be read in the same order in which they occur in source code. If order is important, provide a value here. It will be used to sort your examples in ascending order.</param>
        /// <param name="usage">An example invocation of your program, excluding the command part (provided automatically).</param>
        /// <param name="description">Optional - tell the user what this particular example will do.</param>
        public UsageAttribute(int order, string usage, string description = null)
        {
            _order = order;
            _usage = usage;
            _description = description;
        }

    }
}
