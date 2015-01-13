namespace RCommandLine.Models
{
    using System;
    using System.Reflection;
    using System.Runtime.Remoting.Messaging;
    using Util;

    /// <summary>
    /// Either a named argument or a flag (subtype)
    /// </summary>
    abstract class Parameter : Model
    {
        

        public Maybe<Func<object>> DefaultValueProvider { get; set; }
        public bool HasValue { get; protected set; }

        public PropertyInfo TargetProperty { get; private set; }

        public Type TargetType { get; protected set; }

        protected Parameter(string name, PropertyInfo property, Maybe<object> defaultValue) : base(property.DeclaringType)
        {
            Name = name;

            TargetProperty = property;
            TargetType = property.PropertyType;

            DefaultValueProvider = new Maybe<Func<object>>();
            if (defaultValue.HasValue)
                DefaultValueProvider.Value = () => defaultValue.Value;

            var nullableType = Nullable.GetUnderlyingType(TargetType);
            if (nullableType != null)
            {
                TargetType = nullableType;
                _isNullable = true;
            }
        }

        private readonly bool _isNullable;

        public virtual bool Required { get { return !DefaultValueProvider.HasValue && !_isNullable; } }

        public void ApplyDefaultValue(object target)
        {
            if (DefaultValueProvider.HasValue)
                PushValue(target, DefaultValueProvider.Value(), false, true);
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

        internal static bool Equals(Parameter a, Parameter b)
        {
            return a._isNullable == b._isNullable
                && a.TargetProperty.Equals(b.TargetProperty)
                && Model.Equals(a, b);
        }

        public override bool Equals(object obj)
        {
            var p = obj as Parameter;
            return p != null && Equals(this, p);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = _isNullable.GetHashCode();
                hashCode = (hashCode*397) ^ (TargetProperty != null ? TargetProperty.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (TargetType != null ? TargetType.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
    
}
