﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace RCommandLine.ModelConversion
{
    using Fluent;
    using Models;

    public class FluentModelBuilder<TOptions> where TOptions : class
    {
        private readonly Dictionary<Type, IMixin> _mixins;

        private readonly ParserOptions _options;

        private readonly FluentCommandWrapper<TOptions, TOptions> _root;

        private Command RootCommand { get { return _root.GetCommand(); } }
        internal Type TopType { get; private set; }

        internal FluentModelBuilder(ParserOptions options)
        {
            _options = options;

            _mixins = new Dictionary<Type, IMixin>();
            TopType = typeof(TOptions);

            _root = new FluentCommandWrapper<TOptions, TOptions>(this);
            _root.Name("(root)");
        }

        public FluentModelBuilder<TOptions> BaseCommand<TBase>(Action<IParameterContainer<TBase, TOptions>> configurator) where TBase : class
        {
            var stub = new CommandMixin<TBase, TOptions>(this);
            configurator(stub);

            AddMixin(typeof (TBase), stub);

            return this;
        }

        public FluentModelBuilder<TOptions> Command<TCommand>(Action<IFluentCommand<TCommand, TOptions>> configurator) where TCommand : class
        {
            var wrapper = NewWrapper<TCommand>();
            configurator(wrapper);

            return this;
        }

        public Parser<TOptions> Build()
        {
            return new Parser<TOptions>(_options, RootCommand);
        }

        private void ApplyMixins<TCommand>(FluentCommandWrapper<TCommand, TOptions> wrapper)
            where TCommand : class
        {
            foreach (var mixin in GetAncestors(typeof(TCommand))
                .Where(t => _mixins.ContainsKey(t))
                .Select(t => _mixins[t]))
                mixin.Inject(wrapper);
        }

        private void AddMixin(Type type, IMixin mixin)
        {
            _mixins.Add(type, mixin);
        }

        internal FluentCommandWrapper<TCommand, TOptions> NewWrapper<TCommand>(Command parent = null)
            where TCommand : class
        {
            var wrapper = new FluentCommandWrapper<TCommand, TOptions>(this, parent);
            ApplyMixins(wrapper);
            return wrapper;
        }

        static IEnumerable<Type> GetAncestors(Type type)
        {
            var types = new List<Type>();

            while (true)
            {
                type = type.BaseType;
                if (type == null || type == typeof(object))
                    break;
                types.Add(type);
            }

            return types;
        }

    }
}
