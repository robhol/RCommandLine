namespace RCommandLine.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Parsers;
    using Util;

    /// <summary>
    /// A flag can occur anywhere identified by a char or long name.
    /// </summary>
    class Flag : Parameter
    {
        public char ShortName { get; private set; }
        public bool HasShortName { get; private set; }

        public bool IsList { get; private set; }

        public Flag(char shortName, string name, PropertyInfo property, Maybe<object> defaultValue)
            : base(name, property, defaultValue)
        {
            ShortName = shortName;
            HasShortName = shortName != default(char);
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
            {
                ((IList) TargetProperty.GetValue(target)).Add(value);
            }
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
    }
}
