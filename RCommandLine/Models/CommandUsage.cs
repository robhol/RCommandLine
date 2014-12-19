﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine.Models
{
    class CommandUsage
    {

        public string Usage { get; set; }

        public string Description { get; set; }

        public CommandUsage(string usage, string description)
        {
            Usage = usage;
            Description = description;
        }

    }
}