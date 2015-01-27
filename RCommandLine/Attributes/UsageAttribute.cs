using System;

namespace RCommandLine.Attributes
{
    /// <summary>
    /// Provides a human-readable example invocation of this command with an optional description.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class UsageAttribute : Attribute
    {

        private readonly string _usage;
        private readonly string _description;

        /// <summary>
        /// Attributes are not guaranteed to be read in the same order in which they occur in source code. If order is important, provide a value here. It will be used to sort your examples in ascending order.
        /// </summary>
        public int Order { get; set; }

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
        public UsageAttribute(string usage, string description = null)
        {
            _usage = usage;
            _description = description;
        }

    }
}
