namespace RCommandLine.Attributes
{
    using System;

    /// <summary>
    /// Provides a human-readable name and description for "extra" arguments (ie. ones that are not explicitly defined)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class LabelExtraArgumentsAttribute : ParameterAttribute
    {
        public LabelExtraArgumentsAttribute(string name) : base(name)
        {
            
        }
    }
}