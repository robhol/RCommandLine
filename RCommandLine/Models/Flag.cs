using System.Reflection;

namespace RCommandLine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A flag can occur anywhere identified by a char or long name.
    /// </summary>
    class Flag : Parameter
    {
        public char ShortName { get; private set; }

        public bool IsList { get; private set; }

        public Flag(char shortName, string name, PropertyInfo property, Maybe<object> defaultValue)
            : base(name, property, defaultValue)
        {
            ShortName = shortName;

            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                DefaultValue.Value = Activator.CreateInstance(property.PropertyType);
                TargetType = property.PropertyType.GetGenericArguments().Single();
                IsList = true;
            }

            if (property.PropertyType == typeof (bool) && !DefaultValue.HasValue)
                DefaultValue.Value = false;
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
            return (Name != null) ? ("--" + Name) : ("-" + ShortName);
        }

        public override string DefaultName
        {
            get { return Util.BumpyCaseToHyphenate(TargetProperty.Name); }
        }
    }
}
