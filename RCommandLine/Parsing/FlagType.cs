using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{
    [Flags]
    enum FlagType
    {
        None = 0,
        Short = 1,
        Long = 2,
        Both = 3
    }
}
