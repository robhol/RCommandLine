namespace RCommandLine.Exceptions
{
    using System;

    public class UnrecognizedFlagException : Exception
    {
        public string Flag { get; private set; }

        public UnrecognizedFlagException(string flag, string message = null) : base(message)
        {
            Flag = flag;
        }
    }
}
