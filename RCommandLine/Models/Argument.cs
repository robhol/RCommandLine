using System.Reflection;

namespace RCommandLine
{
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
    }
}
