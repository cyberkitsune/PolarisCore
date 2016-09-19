using System;
using System.Collections.Generic;

namespace Polaris.Packet
{
    public partial class Packet
    {
        // TODO: Maybe find a better readonly structure for this, or just use an array 
        public static readonly Dictionary<ushort, Type> PacketList = new Dictionary<ushort, Type>()
            {
                { 0x0452, typeof(PacketDamage) }
            };
    }
}
