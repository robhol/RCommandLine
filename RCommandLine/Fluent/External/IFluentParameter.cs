namespace RCommandLine.Fluent
{
    public interface IFluentParameter<TTarget>
    {
        /// <summary>
        /// Sets the naming strategy for this parameter.
        /// </summary>
        IFluentParameter<TTarget> Name(NameType nameType);

        /// <summary>
        /// Explicitly sets the parameter's name.
        /// </summary>
        IFluentParameter<TTarget> Name(string name);

        /// <summary>
        /// Sets a description for this parameter. Descriptions are used in generated help texts.
        /// </summary>
        IFluentParameter<TTarget> Description(string description);

        /// <summary>
        /// Marks the parameter as optional.
        /// </summary>
        /// <returns></returns>
        IFluentParameter<TTarget> Optional();

        /// <summary>
        /// Marks the parameter as optional, with a given default value.
        /// </summary>
        /// <param name="default"></param>
        IFluentParameter<TTarget> Optional(TTarget @default);
    }
}