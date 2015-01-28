namespace RCommandLine.Fluent
{
    using System;
    using Models;
    using Parsing;
    using Util;

    class FluentParameterWrapper<TTarget>
    {
        private readonly Parameter _parameter;

        private Parameter Parameter
        {
            get { return _parameter; }
        }

        internal FluentParameterWrapper(Parameter parameter)
        {
            _parameter = parameter;
        }

        public void Name(NameType nameType)
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
        }

        public void Name(string name)
        {
            Parameter.Name = name;
        }

        public void Description(string description)
        {
            Parameter.Description = description;
        }

        public void Optional()
        {
            Parameter.DefaultValueProvider = new Maybe<Func<object>>();
        }

        public void Optional(TTarget @default)
        {
            Parameter.DefaultValueProvider = new Maybe<Func<object>>(() => @default);
        }

    }
}
