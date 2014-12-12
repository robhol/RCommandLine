namespace RCommandLine.Parsing
{
    class InputArgument
    {
        public string Value { get; set; }
        public bool Literal { get; set; }

        public override string ToString()
        {
            return Value + (Literal ? " (literal)" : "");
        }
    }
}
