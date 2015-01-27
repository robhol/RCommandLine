
namespace RCommandLine.Models
{
    using System;

    abstract class Model
    {
        private string _name;

        public string Name
        {
            get { return _name ?? DefaultName; }
            set { _name = value; }
        }

        public abstract string DefaultName { get; }
        public string Description { get; set; }
        public Type EncounteredInType { get; private set; }

        protected Model(Type encounteredInType)
        {
            EncounteredInType = encounteredInType;
        }

        internal static bool Equals(Model a, Model b)
        {
            return string.Equals(a._name, b._name) && a.EncounteredInType == b.EncounteredInType;
        }

        public override bool Equals(object obj)
        {
            var b = obj as Model;
            return b != null && Equals(this, b);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 397 ^ EncounteredInType.GetHashCode();
                return hashCode;
            }
        }
    }
}
