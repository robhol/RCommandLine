using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{
    /// <summary>
    /// Arguments occur in a fixed order after all the flags are dealt with.
    /// </summary>
    class ArgumentElement : ParameterElement
    {
        public int Order { get; private set; }

        public ArgumentElement(ArgumentAttribute a, PropertyInfo property, OptionalAttribute optionalAttributeInfo) 
            : base(property, a.Name, a.Description, optionalAttributeInfo)
        {
            Name = Name ?? property.Name;
            Order = a.GetOrder();
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
