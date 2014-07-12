using System;

namespace RCommandLine
{
    /// <summary>
    /// Contains common properties for command line element attributes.
    /// </summary>
    public abstract class ElementAttribute : Attribute
    {


        protected string _name;
        public string Name { get { return _name; } }
        
        public string Description { get; set; }

    }
}
