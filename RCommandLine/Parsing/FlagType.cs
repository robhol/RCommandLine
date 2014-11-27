using System;

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
