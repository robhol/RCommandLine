using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionalAttribute : Attribute
    {
        public object Default { get; set; }
    }
}
