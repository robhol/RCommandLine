using System;
using System.Reflection;

namespace RCommandLine
{

    /// <summary>
    /// Either a named argument or a flag (subtype)
    /// </summary>
    abstract class Parameter : Model
    {
        

        public Maybe<object> DefaultValue { get; private set; }
        public bool HasValue { get; private set; }

        public PropertyInfo TargetProperty { get; private set; }

        public Type TargetType { get; private set; }
        
        protected Parameter(string name, PropertyInfo property, Maybe<object> defaultValue) : base(property.DeclaringType)
        {
            Name = name;

            TargetProperty = property;
            TargetType = property.PropertyType;

            DefaultValue = defaultValue;

            var nullableType = Nullable.GetUnderlyingType(TargetType);
            if (nullableType != null)
            {
                TargetType = nullableType;
                _isNullable = true;
            }
        }

        private readonly bool _isNullable;

        public bool Required { get { return !DefaultValue.HasValue && !_isNullable; } }

        /// <summary>
        /// Provides the element with an opportunity to initialize, given the prospective output object
        /// </summary>
        public void ApplyDefaultValue(object target)
        {
            if (DefaultValue.HasValue)
                SetValue(target, DefaultValue.Value, false);
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
            TargetProperty.SetValue(target, value, null);
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
