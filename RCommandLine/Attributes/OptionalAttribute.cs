namespace RCommandLine.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class OptionalAttribute : Attribute
    {
        public object Default { get; set; }
    }
}
