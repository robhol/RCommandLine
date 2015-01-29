namespace RCommandLine.Attributes
{
    using System;
    using Parsing;

    /// <summary>
    /// A flag can occur anywhere in the argument string.
    /// It is identified by a char (-f) or a long Name. (--name)
    /// If a Name is not provided (null), it will default to a "flagified" version of the assigned Property's name. (MyProperty => --my-property)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FlagAttribute : ParameterAttribute
    {

        /// <summary>
        /// A unique single-character case-sensitive identifier for this flag.
        /// </summary>
        private readonly char _shortName;

        public FlagAttribute(char shortName, string longName = null) : base(longName)
        {
            _shortName = shortName;
        }

        public FlagAttribute(NameType longName = NameType.Default) : this(default(char), null)
        {
            if (longName == NameType.None)
                throw new ArgumentException("Flags need at least one short or long name.");
        }

        public FlagAttribute(string longName) : this(default(char), longName) { }

        public char GetShortName() //non-property public getter for client code syntax purposes
        {
            return _shortName;
        }

        public override string ToString()
        {
            return (Name != null) ? ("--" + Name) : ("-" + _shortName);
        }

    }
}
