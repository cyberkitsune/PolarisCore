using Polaris.Lib.Packet;

namespace PolarisLib.packet
{
	/// Packets sent by the server and received by the client
	interface ISendPacket
    {
		byte[] ConstructPacket(PacketHeader header);
	}
}
