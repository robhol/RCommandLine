using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{
    /// <summary>
    /// A flag can occur anywhere in the argument string identified by a mandatory char.
    /// If a Name is not provided, it will default to a "flagified" version of the assigned Property. (MyProperty => --my-property)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FlagAttribute : ElementAttribute
    {

        /// <summary>
        /// A unique single-character case-sensitive identifier for this flag, to be used with a hyphen in the argument string.
        /// </summary>
        private char shortName;

        public FlagAttribute(char shortName)
        {
            this.shortName = shortName;
        }

        public char GetShortName() //non-property public getter for client code syntax purposes
        {
            return shortName;
        }

        public override string ToString()
        {
            return (Name != null) ? ("--" + Name) : ("-" + shortName);
        }

    }
}
