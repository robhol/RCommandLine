using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RCommandLine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HasCommandAttribute : Attribute
    {
        private readonly Type _commandOptionsType;

        /// <summary>
        /// Whether or not this command should be the default, if a (sub)command at the same level is not specified.
        /// Note that this will "override" the possibility of the parent command being called with no arguments.
        /// </summary>
        //public bool Default { get; set; }

        public string Name { get; set; }

        public Type CommandOptionsType { get { return _commandOptionsType; } }

        public HasCommandAttribute(Type commandOptionsType)
        {
            _commandOptionsType = commandOptionsType;
        }
    }
}
