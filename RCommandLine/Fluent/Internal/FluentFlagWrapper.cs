namespace RCommandLine.Fluent
{
    using Models;
    using Parsing;

    class FluentFlagWrapper<TTarget> : IFluentFlag<TTarget>
    {

        private readonly Flag _flag;
        private readonly FluentParameterWrapper<TTarget> _parameterWrapper; 

        internal FluentFlagWrapper(Flag flag)
        {
            _flag = flag;
            _parameterWrapper = new FluentParameterWrapper<TTarget>(flag);
        }
        
        public IFluentFlag<TTarget> ShortName(char character)
        {
            _flag.ShortName = character;
            return this;
        }

        public IFluentFlag<TTarget> Name(NameType nameType)
        {
            _parameterWrapper.Name(nameType);
            return this;
        }

        public IFluentFlag<TTarget> Name(string name)
        {
            _parameterWrapper.Name(name);
            return this;
        }

        public IFluentFlag<TTarget> Description(string description)
        {
            _parameterWrapper.Description(description);
            return this;
        }

        public IFluentFlag<TTarget> Optional()
        {
            _parameterWrapper.Optional();
            return this;
        }

        public IFluentFlag<TTarget> Optional(TTarget @default)
        {
            _parameterWrapper.Optional(@default);
            return this;
        }
    }
}