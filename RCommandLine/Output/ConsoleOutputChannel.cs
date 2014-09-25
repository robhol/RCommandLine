using System;

namespace RCommandLine.Output
{
    internal class ConsoleOutputChannel : IOutput
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
