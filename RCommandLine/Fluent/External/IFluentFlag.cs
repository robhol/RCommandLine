namespace RCommandLine.Fluent
{
    public interface IFluentFlag<TTarget> : IFluentParameter<TTarget>
    {
        /// <summary>
        /// Set a character that identifies this flag by itself or as part of a bundle.
        /// </summary>
        IFluentFlag<TTarget> ShortName(char character);
    }
}