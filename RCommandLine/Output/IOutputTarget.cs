namespace RCommandLine.Output
{
    public interface IOutputTarget
    {
        void Write(string s);

        void WriteLine(string s);
    }
}
