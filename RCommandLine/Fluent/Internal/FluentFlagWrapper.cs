namespace RCommandLine.Fluent
{
    using Models;

    class FluentFlagWrapper<TTarget> : FluentParameterWrapper<TTarget>, IFluentFlag<TTarget>
    {

        private readonly Flag _flag;

        internal FluentFlagWrapper(Flag flag) : base(flag)
        {
            _flag = flag;
        }
        
        public IFluentFlag<TTarget> ShortName(char character)
        {
            _flag.ShortName = character;
            return this;
        }

    }
}