using System;
using System.Collections.Generic;

namespace RCommandLine.Fluent
{
    using System.Linq.Expressions;
    using System.Reflection;
    using ModelConversion;
    using Models;
    using Parsers;
    using Util;

    internal abstract class ParameterOwner<T, TOptions> : IParameterContainer<T, TOptions> where T : class where TOptions : class
    {
        private readonly FluentModelBuilder<TOptions> _builder;
        public List<Argument> Arguments { get; private set; }
        public List<Flag> Flags { get; private set; }

        protected ParameterOwner(FluentModelBuilder<TOptions> builder)
        {
            _builder = builder;
            Arguments = new List<Argument>();
            Flags = new List<Flag>();
        }

        public IParameterContainer<T, TOptions> Argument<TTarget>(Expression<Func<T, TTarget>> property, Action<IFluentArgument<TTarget>> configurator = null)
        {
            Arguments.Add(BuildArgument(Arguments.Count, property, configurator));
            return this;
        }

        public IParameterContainer<T, TOptions> Flag<TTarget>(Expression<Func<T, TTarget>> property, Action<IFluentFlag<TTarget>> configurator = null)
        {
            Flags.Add(BuildFlag(property, configurator));
            return this;
        }

        public Parser<TOptions> Build()
        {
            return _builder.Build();
        }

        public static PropertyInfo GetPropertyInfoFromExpression<TSource, TTarget>(Expression<Func<TSource, TTarget>> exp)
        {
            var body = exp as Expression;
            if (body is LambdaExpression)
            {
                body = (body as LambdaExpression).Body;
            }

            if (body.NodeType != ExpressionType.MemberAccess)
                throw new ArgumentException("Expression does not point to a property");

            var member = ((MemberExpression)body).Member;
            var propertyInfo = member as PropertyInfo;

            if (propertyInfo == null)
                throw new ArgumentException("Expression does not point to a property");

            return propertyInfo;
        }

        public static Argument BuildArgument<TTarget>(int order, Expression<Func<T, TTarget>> expr, Action<IFluentArgument<TTarget>> configurator)
        {
            var property = GetPropertyInfoFromExpression(expr);
            var arg = new Argument(order, null, property, new Maybe<object>());

            if (configurator != null)
                configurator(new FluentArgumentWrapper<TTarget>(arg));

            return arg;
        }

        public static Flag BuildFlag<TTarget>(Expression<Func<T, TTarget>> expr, Action<IFluentFlag<TTarget>> configurator)
        {
            var property = GetPropertyInfoFromExpression(expr);
            var flag = new Flag(default(char), null, property, new Maybe<object>());

            if (configurator != null)
                configurator(new FluentFlagWrapper<TTarget>(flag));

            return flag;
        }

    }

    



}
