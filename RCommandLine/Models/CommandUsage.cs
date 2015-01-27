namespace RCommandLine.Models
{
    class CommandUsage
    {

        public string Usage { get; private set; }

        public string Description { get; set; }

        public int Order { get; set; }

        public CommandUsage(string usage)
        {
            Usage = usage;
        }

        private bool Equals(CommandUsage other)
        {
            return string.Equals(Usage, other.Usage) && string.Equals(Description, other.Description) && Order == other.Order;
        }

        public override bool Equals(object obj)
        {
            var u = obj as CommandUsage;
            return u != null && Equals(u);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Usage != null ? Usage.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Order;
                return hashCode;
            }
        }
    }
}
