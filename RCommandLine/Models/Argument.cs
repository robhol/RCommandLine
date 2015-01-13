namespace RCommandLine.Models
{
    using System.Collections.Concurrent;
    using System.Reflection;
    using Util;

    /// <summary>
    /// Arguments occur in attr fixed order after all the flags are dealt with.
    /// </summary>
    class Argument : Parameter
    {
        public int Order { get; private set; }

        public Argument(int order, string name, PropertyInfo property, Maybe<object> defaultValue) 
            : base(name, property, defaultValue)
        {
            Name = name ?? property.Name;
            Order = order;
        }

        public override string ToString()
        {
            return string.Format("{0} (#{1})", Name, Order);
        }

        public override string DefaultName
        {
            get { return TargetProperty.Name; }
        }

        internal static bool Equals(Argument a, Argument b)
        {
            return a.Order == b.Order && Parameter.Equals(a, b);
        }

        public override bool Equals(object obj)
        {
            var a = obj as Argument;
            return a != null && Equals(this, a);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ Order;
            }
        }
    }
}
