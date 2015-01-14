using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using RCommandLine.Parsers;

namespace RCommandLine.ModelConversion
{
    using Fluent;
    using Models;

    public class FluentModelBuilder<TOptions> where TOptions : class
    {
        public Type TopType { get; private set; }
        
        private readonly Dictionary<Type, IMixin> _mixins;

        private readonly ParserOptions _options;

        private readonly FluentCommandWrapper<TOptions, TOptions> _root;

        internal Command RootCommand { get { return _root.GetCommand(); } }

        internal FluentModelBuilder(ParserOptions options)
        {
            _options = options;
            _root = new FluentCommandWrapper<TOptions, TOptions>(this);
            _root.Name("(root)");
            _mixins = new Dictionary<Type, IMixin>();

            TopType = typeof(TOptions);
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

        internal void ApplyMixins<TCommand>(FluentCommandWrapper<TCommand, TOptions> wrapper)
            where TCommand : class
        {
            foreach (var mixin in GetAncestors(typeof(TCommand))
                .Where(t => _mixins.ContainsKey(t))
                .Select(t => _mixins[t]))
                mixin.Inject(wrapper);
        }

        internal void AddMixin(Type type, IMixin mixin)
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
