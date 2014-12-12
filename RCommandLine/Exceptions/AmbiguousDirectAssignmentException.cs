namespace RCommandLine.Exceptions
{
    using System;

    public class AmbiguousDirectAssignmentException : Exception
    {
        public string Argument { get; private set; }

        public AmbiguousDirectAssignmentException(string argument, string message) : base(message)
        {
            Argument = argument;
        }
    }
}
