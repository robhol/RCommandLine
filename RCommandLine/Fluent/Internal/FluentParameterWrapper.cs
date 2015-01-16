namespace RCommandLine.Fluent
{
    using System;
    using Models;
    using Util;

    class FluentParameterWrapper<TTarget> : IFluentParameter<TTarget>
    {
        private readonly Parameter _parameter;

        internal Parameter Parameter
        {
            get { return _parameter; }
        }

        internal FluentParameterWrapper(Parameter parameter)
        {
            _parameter = parameter;
        }

        public IFluentParameter<TTarget> Name(NameType nameType)
        {
            switch (nameType)
            {
                case NameType.None:
                    Parameter.Name = string.Empty;
                    break;

                case NameType.Default:
                    Parameter.Name = null;
                    break;
            }

            return this;
        }

        public IFluentParameter<TTarget> Name(string name)
        {
            Parameter.Name = name;
            return this;
        }

        public IFluentParameter<TTarget> Description(string description)
        {
            Parameter.Description = description;
            return this;
        }

        public IFluentParameter<TTarget> Optional()
        {
            Parameter.DefaultValueProvider = new Maybe<Func<object>>();
            return this;
        }

        public IFluentParameter<TTarget> Optional(TTarget @default)
        {
            Parameter.DefaultValueProvider = new Maybe<Func<object>>(() => @default);
            return this;
        }

    }
}
