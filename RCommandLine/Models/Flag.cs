namespace RCommandLine.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Util;

    /// <summary>
    /// A flag can occur anywhere identified by a char or long name.
    /// </summary>
    internal class Flag : Parameter
    {
        public char ShortName { get; set; }
        public bool HasShortName { get { return ShortName != default(char); } }

        public bool IsList { get; private set; }

        public Flag(char shortName, string name, PropertyInfo property, Maybe<object> defaultValue)
            : base(name, property, defaultValue)
        {
            ShortName = shortName;
            Name = name;
        
            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                DefaultValueProvider.Value = GetEmptyListProvider(property.PropertyType);
                TargetType = property.PropertyType.GetGenericArguments().Single();
                IsList = true;
            }

            if (property.PropertyType == typeof (bool) && !DefaultValueProvider.HasValue)
                DefaultValueProvider.Value = () => false;
        }

        static Func<object> GetEmptyListProvider(Type listType)
        {
            return () => Activator.CreateInstance(listType);
        }

        public override string HelpTextIdentifier
        {
            get
            {
                return Util.JoinNotNulls("|", new[]
                    {
                        ShortName != default(char) ? ("-" + ShortName) : null,
                        "--" + Name
                    });
            }
        }

        public override void PushValue(object target, object value, bool updateHasValue = true, bool direct = false)
        {
            if (IsList && !direct)
                ((IList) TargetProperty.GetValue(target)).Add(value);
            else
                base.PushValue(target, value, updateHasValue);
        }

        public override string ToString()
        {
            return ToString("--", "-");
        }

        public string ToString(ParserOptions options, bool preferShort = false)
        {
            return ToString(options.DefaultLongFlagHeader, options.DefaultShortFlagHeader, preferShort);
        }

        public string ToString(string longNameHeader, string shortNameHeader, bool preferShort = false)
        {
            return !preferShort && !string.IsNullOrEmpty(longNameHeader) 
                ? longNameHeader    + Name 
                : shortNameHeader   + ShortName;
        }

        public override string DefaultName
        {
            get { return Util.BumpyCaseToHyphenate(TargetProperty.Name); }
        }

        protected bool Equals(Flag other)
        {
            return base.Equals(other) && ShortName == other.ShortName && HasShortName.Equals(other.HasShortName) && IsList.Equals(other.IsList);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Flag;

            if (other == null)
                return false;

            return ShortName == other.ShortName && ((Parameter) this).Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ ShortName.GetHashCode();
                hashCode = (hashCode*397) ^ HasShortName.GetHashCode();
                hashCode = (hashCode*397) ^ IsList.GetHashCode();
                return hashCode;
            }
        }
    }
}
