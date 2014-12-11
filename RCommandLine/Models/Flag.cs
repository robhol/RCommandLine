using System.Reflection;

namespace RCommandLine
{
    /// <summary>
    /// A flag can occur anywhere identified by a char or long name.
    /// </summary>
    class Flag : Parameter
    {
        public char ShortName { get; private set; }

        public Flag(char shortName, string name, PropertyInfo property, Maybe<object> defaultValue)
            : base(name, property, defaultValue)
        {
            ShortName = shortName;

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
