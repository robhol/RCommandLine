using System;

namespace RCommandLine
{
    /// <summary>
    /// Contains common properties for command line element attributes.
    /// </summary>
    public abstract class ElementAttribute : Attribute
    {

        internal ElementAttribute(string name = null)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public string Description { get; set; }

    }
}
