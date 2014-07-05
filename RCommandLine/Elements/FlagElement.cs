using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{
    /// <summary>
    /// A flag can occur anywhere identified by a mandatory char or an optional long name.
    /// </summary>
    class FlagElement : ParameterElement
    {
        public char ShortName { get; private set; }

        public FlagElement(FlagAttribute f, PropertyInfo property, OptionalAttribute optionalAttributeInfo)
            : base(property, f.Name, f.Description, optionalAttributeInfo)
        {
            ShortName = f.GetShortName();
            Name = Name ?? Util.BumpyCaseToHyphenate(property.Name);

            if (property.PropertyType == typeof (bool))
            {
                Required = false;
                DefaultValue = DefaultValue ?? false;
            }
        }

        public override string ToString()
        {
            return (Name != null) ? ("--" + Name) : ("-" + ShortName);
        }

    }
}
