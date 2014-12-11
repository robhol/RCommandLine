using System;

namespace RCommandLine
{
    /// <summary>
    /// Contains common properties for command line element attributes.
    /// </summary>
    public abstract class ParameterAttribute : Attribute
    {
        internal ParameterAttribute(string name = null)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public string Description { get; set; }
    }
}
