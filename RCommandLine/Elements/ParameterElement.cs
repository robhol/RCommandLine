using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{

    /// <summary>
    /// Either a named argument or a flag (subtype)
    /// </summary>

    class ParameterElement : Element
    {
        public bool Required { get; set; }
        public object DefaultValue { get; set; }
        public bool HasValue { get; private set; }

        public PropertyInfo TargetProperty { get; private set; }
        public Type TargetType { get; private set; }

        public ParameterElement(PropertyInfo prop, string name, string description, OptionalAttribute optionalAttributeInfo)
        {
            Name = name;
            Description = description;

            TargetProperty = prop;
            TargetType = prop.PropertyType;
            
            Required = true;

            var nullableType = Nullable.GetUnderlyingType(TargetType);
            if (nullableType != null)
            {
                TargetType = nullableType;
                Required = false;
            }

            if (optionalAttributeInfo == null) 
                return;
            
            DefaultValue = optionalAttributeInfo.Default;
            Required = false;
        }

        /// <summary>
        /// Provides the element with an opportunity to initialize, given the prospective output object
        /// </summary>
        public void Hydrate(object target)
        {
            if (DefaultValue != null)
                SetValue(target, DefaultValue, false);
        }

        public void ConvertAndSetValue(object target, string arg)
        {
            object objectArg;

            if (TargetType == typeof (string))
                objectArg = arg;
            else
                try
                {
                    objectArg = Convert.ChangeType(arg, TargetType, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    throw new InvalidCastException("Could not convert \"" + arg + "\" to " + TargetType.Name, e);
                }

            SetValue(target, objectArg);
        }

        public void SetValue(object target, object value, bool updateHasValue = true)
        {
            TargetProperty.SetValue(target, value);
            HasValue = HasValue || updateHasValue;
        }

        public override string ToString()
        {
            return Name;
        }

    }
    
}
