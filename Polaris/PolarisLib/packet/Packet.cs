using System;

namespace Polaris.Packet
{
    // TODO: For packets it might be better to use unsafe code and Marshal + packed structures to parse the data out
    public partial class Packet
    {
        public PacketHeader header;
        public byte[] data;

        public Packet(byte[] pkt)
        {
            this.header = new PacketHeader(pkt);
            Array.Copy(pkt, 0x8, this.data, 0x0, pkt.Length - 0x8);
        }

        protected virtual void ParseData(byte[] data)
        {

        }
    }
}
