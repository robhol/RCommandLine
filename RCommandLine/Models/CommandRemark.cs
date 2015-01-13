namespace RCommandLine.Models
{
    public class CommandRemark
    {
        private readonly string _remark;

        public CommandRemark(string remark)
        {
            _remark = remark;
        }

        public string Remark
        {
            get { return _remark; }
        }

        public int Order { get; set; }

        protected bool Equals(CommandRemark other)
        {
            return string.Equals(_remark, other._remark) && Order == other.Order;
        }

        public override bool Equals(object obj)
        {
            var r = obj as CommandRemark;
            return r != null && Equals(r);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_remark != null ? _remark.GetHashCode() : 0)*397) ^ Order;
            }
        }
    }
}