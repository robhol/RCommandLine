namespace RCommandLine.Fluent
{
    using System;

    public interface IFluentCommand<T, TOptions> : IParameterContainer<T, TOptions> where T : class where TOptions : class
    {

        /// <summary>
        /// Set the naming strategy for this command.
        /// </summary>
        IFluentCommand<T, TOptions> Name(NameType type);

        /// <summary>
        /// Provide an explicit name for this command.
        /// </summary>
        IFluentCommand<T, TOptions> Name(string name);

        /// <summary>
        /// Provide a human-readable explanation of this command.
        /// </summary>
        IFluentCommand<T, TOptions> Description(string description);

        /// <summary>
        /// Whether or not this should be hidden from the Parser.GetUsage method.
        /// </summary>
        IFluentCommand<T, TOptions> Hidden(bool hidden = true);

        /// <summary>
        /// If you will be processing "extra" arguments (not defined in the options object), use this method to explain their usage to your users.
        /// </summary>
        IFluentCommand<T, TOptions> LabelExtraArguments(string name, string description);


        /// <summary>
        /// Define a new command under the current one.
        /// </summary>
        /// <typeparam name="TCommand">The command/options type of the new command.</typeparam>
        /// <param name="configurator">An expression fluently defining the traits of the new command.</param>
        /// <param name="name">The command's name - defaults to the name of TCommand after removing a trailing "Command" or "Options"</param>
        /// <param name="description">A helpful description for use in help texts.</param>
        IFluentCommand<T, TOptions> Command<TCommand>(Action<IFluentCommand<TCommand, TOptions>> configurator) where TCommand : class;

        /// <summary>
        /// Defines a human-readable usage example for the current command.
        /// </summary>
        /// <param name="invocation">An example invocation of your program, excluding the command part (provided automatically).</param>
        /// <param name="description">Optional - tell the user what this particular example will do.</param>
        IFluentCommand<T, TOptions> Usage(string invocation, string description = null);

        /// <summary>
        /// Defines a remark to be displayed in help texts.
        /// </summary>
        IFluentCommand<T, TOptions> Remark(string remark);


    }
}