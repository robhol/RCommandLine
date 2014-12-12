
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
    }
}
