namespace RCommandLine.Fluent
{
    public interface IFluentArgument<TTarget>
    {
        /// <summary>
        /// Explicitly set the ordering value for this argument.
        /// Defaults to an autoincrementing value by which arguments are sorted in descending order.
        /// </summary>
        IFluentArgument<TTarget> Order(int order);

        /// <summary>
        /// Sets the naming strategy for this argument.
        /// </summary>
        IFluentArgument<TTarget> Name(NameType nameType);

        /// <summary>
        /// Explicitly sets the argument's name.
        /// </summary>
        IFluentArgument<TTarget> Name(string name);

        /// <summary>
        /// Sets a description for this argument. Descriptions are used in generated help texts.
        /// </summary>
        IFluentArgument<TTarget> Description(string description);

        /// <summary>
        /// Marks the argument as optional.
        /// </summary>
        /// <returns></returns>
        IFluentArgument<TTarget> Optional();

        /// <summary>
        /// Marks the argument as optional, with a given default value.
        /// </summary>
        /// <param name="default"></param>
        IFluentArgument<TTarget> Optional(TTarget @default);
    }
}