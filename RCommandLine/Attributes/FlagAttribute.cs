using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{
    /// <summary>
    /// A flag can occur anywhere in the argument string.
    /// It is identified by a char (-f) or a long Name. (--name)
    /// If a Name is not provided (null), it will default to a "flagified" version of the assigned Property's name. (MyProperty => --my-property)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FlagAttribute : ElementAttribute
    {

        /// <summary>
        /// A unique single-character case-sensitive identifier for this flag, to be used with a hyphen in the argument string.
        /// </summary>
        private readonly char _shortName;

        public FlagAttribute(char shortName, string longName)
        {
            _shortName = shortName;
            _name = longName;
        }

        public FlagAttribute(char shortName) : this(shortName, null) { }
        public FlagAttribute(string longName) : this(default(char), longName) { }

        public char GetShortName() //non-property public getter for client code syntax purposes
        {
            return _shortName;
        }

        public override string ToString()
        {
            return (Name != null) ? ("--" + Name) : ("-" + _shortName);
        }

    }
}
