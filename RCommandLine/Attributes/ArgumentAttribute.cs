namespace RCommandLine.Attributes
{
    using System;

    /// <summary>
    /// Arguments occur in a fixed order after all the flags are dealt with.
    /// The fixed order means that more than one optional ArgumentAttribute should be avoided.
    /// If a Name is not provided, it will default to the property name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ArgumentAttribute : ParameterAttribute
    {
        private readonly int _order;

        public ArgumentAttribute(int order)
        {
            _order = order;
        }

        public int GetOrder()
        {
            return _order;
        }
    }
}
