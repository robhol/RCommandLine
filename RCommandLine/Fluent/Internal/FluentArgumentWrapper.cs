namespace RCommandLine.Fluent
{
    using Models;
    using Parsing;

    class FluentArgumentWrapper<TTarget> : IFluentArgument<TTarget>
    {

        private readonly Argument _argument;
        private readonly FluentParameterWrapper<TTarget> _parameterWrapper; 

        internal FluentArgumentWrapper(Argument argument)
        {
            _argument = argument;
            _parameterWrapper = new FluentParameterWrapper<TTarget>(argument);
        }

        public IFluentArgument<TTarget> Order(int order)
        {
            _argument.Order = order;
            return this;
        }

        public IFluentArgument<TTarget> Name(NameType nameType)
        {
            _parameterWrapper.Name(nameType);
            return this;
        }

        public IFluentArgument<TTarget> Name(string name)
        {
            _parameterWrapper.Name(name);
            return this;
        }

        public IFluentArgument<TTarget> Description(string description)
        {
            _parameterWrapper.Description(description);
            return this;
        }

        public IFluentArgument<TTarget> Optional()
        {
            _parameterWrapper.Optional();
            return this;
        }

        public IFluentArgument<TTarget> Optional(TTarget @default)
        {
            _parameterWrapper.Optional(@default);
            return this;
        }

    }
}