using System;
using System.Reflection;

namespace RCommandLine
{

    /// <summary>
    /// Either a named argument or a flag (subtype)
    /// </summary>
    abstract class CommonParameterElement : Element
    {
        public bool Required { get; set; }
        public object DefaultValue { get; set; }
        public bool HasValue { get; private set; }

        public PropertyInfo TargetProperty { get; private set; }
        public Type TargetType { get; private set; }

        public Type EncounteredInType { get; private set; }

        protected CommonParameterElement(ElementAttribute attribute, PropertyInfo property, OptionalAttribute optionalAttributeInfo)
        {
            Name = attribute.Name;
            Description = attribute.Description;

            TargetProperty = property;
            TargetType = property.PropertyType;

            EncounteredInType = property.DeclaringType;
            
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

        public virtual string HelpTextIdentifier { get { return Name; } }

        public virtual string GetHelpTextRepresentation()
        {
            return string.Format(Required ? "{0}" : "[{0}]", HelpTextIdentifier);
        }

        public override string ToString()
        {
            return Name;
        }

    }
    
}
