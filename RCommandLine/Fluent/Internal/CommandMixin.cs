using System;

namespace RCommandLine.Fluent
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using ModelConversion;
    using Models;

    internal class CommandMixin<T, TOptions> : ParameterOwner<T, TOptions>, IMixin where T : class where TOptions : class
    {

        private List<Argument> _arguments;
        private List<Flag> _flags;

        public CommandMixin(FluentModelBuilder<TOptions> builder) : base(builder)
        {
            _arguments = new List<Argument>();
            _flags = new List<Flag>();
        }

        public void Inject(IMixinInjectionTarget cmd)
        {
            foreach (var argument in _arguments)
                cmd.AddArgument(argument);

            foreach (var flag in _flags)
                cmd.AddFlag(flag);
        }

    }

}
