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
    }
}