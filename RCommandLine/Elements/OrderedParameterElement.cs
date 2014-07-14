using System.Reflection;

namespace RCommandLine
{
    /// <summary>
    /// Parameters occur in attr fixed order after all the flags are dealt with.
    /// </summary>
    class OrderedParameterElement : CommonParameterElement
    {
        public int Order { get; private set; }

        public OrderedParameterElement(OrderedParameterAttribute attribute, PropertyInfo property, OptionalAttribute optionalAttributeInfo) 
            : base(attribute, property, optionalAttributeInfo)
        {
            Name = Name ?? property.Name;
            Order = attribute.GetOrder();
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
