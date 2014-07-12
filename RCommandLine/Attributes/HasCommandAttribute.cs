using System;

namespace RCommandLine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HasCommandAttribute : ElementAttribute
    {
        private readonly Type _commandOptionsType;

        /// <summary>
        /// Whether or not this should be hidden from the CommandParser.GetUsage method.
        /// </summary>
        public bool Hidden { get; set; }

        public Type CommandOptionsType { get { return _commandOptionsType; } }

        public HasCommandAttribute(Type commandOptionsType, string name = null)
        {
            _commandOptionsType = commandOptionsType;
            _name = name;
        }
    }
}
