using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{
    /// <summary>
    /// Contains common properties for command line element attributes.
    /// </summary>
    public abstract class ElementAttribute : Attribute
    {


        protected string _name;
        public string Name { get { return _name; } }
        
        public string Description { get; set; }

    }
}
