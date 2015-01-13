namespace RCommandLine.Fluent
{
    using Models;

    interface IMixinInjectionTarget
    {
        void AddArgument(Argument argument);

        void AddFlag(Flag flag);
    }
}