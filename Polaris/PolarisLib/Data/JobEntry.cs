using System;

namespace Polaris.Data
{
    public class JobEntry
    {
        public ushort Level { get; set; }
        public ushort Level2 { get; set; } // Usually the same as the above, what is this used for?
        public uint EXP { get; set; }
    }
}
