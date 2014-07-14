using System.Reflection;

namespace RCommandLine
{
    /// <summary>
    /// A flag can occur anywhere identified by a mandatory char or an optional long name.
    /// </summary>
    class FlagElement : CommonParameterElement
    {
        public char ShortName { get; private set; }

        public FlagElement(FlagAttribute f, PropertyInfo property, OptionalAttribute optionalAttributeInfo)
            : base(f, property, optionalAttributeInfo)
        {
            ShortName = f.GetShortName();
            if (f.Name == null)
                Name = Name ?? Util.BumpyCaseToHyphenate(property.Name);
            else
                Name = f.Name == "" ? null : f.Name; //bit of haxing - "" given in flag means no name, whereas null means "autodetect"

            if (property.PropertyType == typeof (bool))
            {
                Required = false;
                DefaultValue = DefaultValue ?? false;
            }
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

    }
}
