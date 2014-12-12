namespace RCommandLine.Output
{
    #region

    using System;

    #endregion

    internal class ConsoleOutputChannel : IOutputTarget
    {
        private static readonly ConsoleOutputChannel CocInstance = new ConsoleOutputChannel();
        public static ConsoleOutputChannel Instance { get { return CocInstance; } }

        public void Write(string s)
        {
            Console.Write(s);
        }

        public void WriteLine(string s)
        {
            Console.WriteLine(s);
        }
    }
}
