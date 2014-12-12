using System;
using System.Reflection;

namespace RCommandLine
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Either a named argument or a flag (subtype)
    /// </summary>
    abstract class Parameter : Model
    {
        

        public Maybe<object> DefaultValue { get; private set; }
        public bool HasValue { get; protected set; }

        public PropertyInfo TargetProperty { get; private set; }

        public Type TargetType { get; protected set; }

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

        public virtual bool Required { get { return !DefaultValue.HasValue && !_isNullable; } }

        public void ApplyDefaultValue(object target)
        {
            if (DefaultValue.HasValue)
                PushValue(target, DefaultValue.Value, false, true);
        }

        public void ConvertAndPushValue(object target, string arg)
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

            PushValue(target, objectArg);
        }

        public virtual void PushValue(object target, object value, bool updateHasValue = true, bool direct = false)
        {
            TargetProperty.SetValue(target, value, null);
            HasValue = HasValue || updateHasValue;
        }

        public virtual string HelpTextIdentifier { get { return Name; } }

        public virtual string GetHelpTextRepresentation()
        {
            return GetHelpTextRepresentation(Required, HelpTextIdentifier);
        }

        public static string GetHelpTextRepresentation(bool required, string identifier)
        {
            return string.Format(required ? "{0}" : "[{0}]", identifier);
        }

        public override string ToString()
        {
            return Name;
        }

    }
    
}
