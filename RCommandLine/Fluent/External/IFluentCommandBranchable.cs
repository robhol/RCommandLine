namespace RCommandLine.Fluent
{
    using System;

    public interface IFluentCommandBranchable<T, TOptions> : IFluentCommand<T, TOptions> where T : class where TOptions : class
    {

        /// <summary>
        /// Set the naming strategy for this command.
        /// </summary>
        IFluentCommandBranchable<T, TOptions> Name(NameType type);

        /// <summary>
        /// Provide an explicit name for this command.
        /// </summary>
        IFluentCommandBranchable<T, TOptions> Name(string name);

        /// <summary>
        /// Provide a human-readable explanation of this command.
        /// </summary>
        IFluentCommandBranchable<T, TOptions> Description(string description);

        /// <summary>
        /// Whether or not this should be hidden from the Parser.GetUsage method.
        /// </summary>
        IFluentCommandBranchable<T, TOptions> Hidden(bool hidden = true);

        /// <summary>
        /// If you will be processing "extra" arguments (not defined in the options object), use this method to explain their usage to your users.
        /// </summary>
        IFluentCommandBranchable<T, TOptions> LabelExtraArguments(string name, string description);


        /// <summary>
        /// Define a new command under the current one.
        /// </summary>
        /// <typeparam name="TCommand">The command/options type of the new command.</typeparam>
        /// <param name="configurator">An expression fluently defining the traits of the new command.</param>
        /// <param name="name">The command's name - defaults to the name of TCommand after removing a trailing "Command" or "Options"</param>
        /// <param name="description">A helpful description for use in help texts.</param>
        IFluentCommandBranchable<T, TOptions> Command<TCommand>(Action<IFluentCommandBranchable<TCommand, TOptions>> configurator) where TCommand : class;

        /// <summary>
        /// Defines a human-readable usage example for the current command.
        /// </summary>
        /// <param name="invocation">An example invocation of your program, excluding the command part (provided automatically).</param>
        /// <param name="description">Optional - tell the user what this particular example will do.</param>
        IFluentCommandBranchable<T, TOptions> Usage(string invocation, string description = null);

        /// <summary>
        /// Defines a remark to be displayed in help texts.
        /// </summary>
        IFluentCommandBranchable<T, TOptions> Remark(string remark);


    }
}