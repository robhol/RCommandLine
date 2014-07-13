using System.Reflection;

namespace RCommandLine
{
    /// <summary>
    /// Parameters occur in a fixed order after all the flags are dealt with.
    /// </summary>
    class OrderedParameterElement : ArgumentElement
    {
        public int Order { get; private set; }

        public OrderedParameterElement(OrderedParameterAttribute a, PropertyInfo property, OptionalAttribute optionalAttributeInfo) 
            : base(property, a.Name, a.Description, optionalAttributeInfo)
        {
            Name = Name ?? property.Name;
            Order = a.GetOrder();
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
