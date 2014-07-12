using System;

namespace RCommandLine
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionalAttribute : Attribute
    {
        public object Default { get; set; }
    }
}
