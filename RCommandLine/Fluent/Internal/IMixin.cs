namespace RCommandLine.Fluent
{
    using Models;

    interface IMixin
    {
        void Inject(IMixinInjectionTarget target);
    }
}