using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class RemarkAttribute : Attribute
    {
        private readonly string _remark;

        public RemarkAttribute(string remark)
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
