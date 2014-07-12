using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{
    /// <summary>
    /// Parameters occur in a fixed order after all the flags are dealt with.
    /// </summary>
    class ParameterElement : ArgumentElement
    {
        public int Order { get; private set; }

        public ParameterElement(ParameterAttribute a, PropertyInfo property, OptionalAttribute optionalAttributeInfo) 
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
