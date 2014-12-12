namespace RCommandLine.Attributes
{
    using System;

    /// <summary>
    /// Contains common properties for command line element attributes.
    /// </summary>
    public abstract class ParameterAttribute : Attribute
    {
        private readonly string _name;

        internal ParameterAttribute(string name = null)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Description { get; set; }
    }
}
