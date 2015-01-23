namespace RCommandLine.Fluent
{
    public interface IFluentFlag<TTarget>
    {
        /// <summary>
        /// Set a character that identifies this flag by itself or as part of a bundle.
        /// </summary>
        IFluentFlag<TTarget> ShortName(char character);

        /// <summary>
        /// Sets the naming strategy for this parameter.
        /// </summary>
        IFluentFlag<TTarget> Name(NameType nameType);

        /// <summary>
        /// Explicitly sets the parameter's name.
        /// </summary>
        IFluentFlag<TTarget> Name(string name);

        /// <summary>
        /// Sets a description for this parameter. Descriptions are used in generated help texts.
        /// </summary>
        IFluentFlag<TTarget> Description(string description);

        /// <summary>
        /// Marks the parameter as optional.
        /// </summary>
        /// <returns></returns>
        IFluentFlag<TTarget> Optional();

        /// <summary>
        /// Marks the parameter as optional, with a given default value.
        /// </summary>
        /// <param name="default"></param>
        IFluentFlag<TTarget> Optional(TTarget @default);
    }
}