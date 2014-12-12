﻿namespace RCommandLine
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class LabelExtraArgumentsAttribute : ParameterAttribute
    {
        public LabelExtraArgumentsAttribute(string name) : base(name)
        {
            
        }
    }
}