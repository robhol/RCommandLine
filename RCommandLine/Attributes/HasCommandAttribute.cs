namespace RCommandLine.Attributes
{
    using System;

    /// <summary>
    /// Defines a "subcommand" for the current command/options object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HasCommandAttribute : ParameterAttribute
    {
        private readonly Type _commandOptionsType;

        /// <summary>
        /// Whether or not this should be hidden from the Parser.GetUsage method.
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// The type produced when executing the command.
        /// </summary>
        public Type CommandOptionsType { get { return _commandOptionsType; } }

        public HasCommandAttribute(Type commandOptionsType, string name = null) : base(name)
        {
            _commandOptionsType = commandOptionsType;
        }
    }
}
