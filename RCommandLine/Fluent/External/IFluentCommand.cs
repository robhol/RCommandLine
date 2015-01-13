﻿namespace RCommandLine.Fluent
{
    using System;
    using System.Linq.Expressions;
    using Parsers;

    public interface IFluentCommand<T, TOptions> where T : class where TOptions : class
    {

        /// <summary>
        /// Define a new argument on this command/options type.
        /// </summary>
        /// <param name="property">Select target property for this argument</param>
        /// <param name="configurator">Lambda expression for configuring the argument</param>
        IFluentCommand<T, TOptions> Argument<TTarget>(Expression<Func<T, TTarget>> property, Action<FluentParameterWrapper<TTarget>> configurator = null);

        /// <summary>
        /// Define a new flag on this command/options type.
        /// </summary>
        /// <param name="property">Select target property for this flag</param>
        /// <param name="configurator">Lambda expression for configuring the flag</param>
        IFluentCommand<T, TOptions> Flag<TTarget>(Expression<Func<T, TTarget>> property, Action<FluentFlagWrapper<TTarget>> configurator = null);

        /// <summary>
        /// Produce a usable Parser object.
        /// </summary>
        Parser<TOptions> Build();
    }
}